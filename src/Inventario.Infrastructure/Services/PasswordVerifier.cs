using System.Security.Cryptography;
using System.Text;

namespace Inventario.Infrastructure.Services;

/// <summary>
/// Verificación de contraseñas contra la tabla <c>usuarios</c> de emios301.
/// Los hashes están en formato crypt(3) (los mismos que genera la función
/// <c>crypt()</c> de PHP que usa la app webemios):
///   $1$...  MD5-crypt
///   $5$...  SHA-256-crypt
///   $6$...  SHA-512-crypt
/// Implementación verificada contra los vectores oficiales de php-src y contra
/// la salida real de PHP 8.3 (crypt()).
/// </summary>
public interface IPasswordVerifier
{
    bool Verificar(string passwordPlano, string? hashAlmacenado);
}

public class LegacyPasswordVerifier : IPasswordVerifier
{
    private const string CryptAlphabet = "./0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public bool Verificar(string passwordPlano, string? hashAlmacenado)
    {
        if (string.IsNullOrWhiteSpace(hashAlmacenado))
            return false;

        var hash = hashAlmacenado.Trim();

        string calculado;
        try
        {
            if (hash.StartsWith("$1$", StringComparison.Ordinal))
                calculado = MD5Crypt(passwordPlano, hash);
            else if (hash.StartsWith("$5$", StringComparison.Ordinal))
                calculado = SHA256Crypt(passwordPlano, hash);
            else if (hash.StartsWith("$6$", StringComparison.Ordinal))
                calculado = SHA512Crypt(passwordPlano, hash);
            else
                return false;
        }
        catch
        {
            return false;
        }

        // La comparación se hace solo con el digest (último segmento tras el '$' final):
        // el hash almacenado puede incluir "rounds=N$" explícito (p. ej. rounds=5000)
        // mientras que el cálculo lo omite cuando es el valor por defecto, aunque el
        // digest resultante sea idéntico.
        var digestCalculado = UltimoSegmento(calculado);
        var digestAlmacenado = UltimoSegmento(hash);

        return digestCalculado is not null
            && digestAlmacenado is not null
            && string.Equals(digestCalculado, digestAlmacenado, StringComparison.Ordinal);
    }

    /// <summary>Devuelve el texto tras el último '$' (el digest), o null si no hay.</summary>
    private static string? UltimoSegmento(string s)
    {
        var idx = s.LastIndexOf('$');
        return (idx < 0 || idx == s.Length - 1) ? null : s[(idx + 1)..];
    }

    // ===================== MD5-crypt ($1$) =====================
    private static string MD5Crypt(string password, string stored)
    {
        var salt = ExtraerSal(stored, 3, 8); // $1$<sal>$
        var pwd = Utf8(password);
        var saltB = Utf8(salt);

        var alt = MD5.HashData(Concat(pwd, saltB, pwd)); // MD5(pwd + salt + pwd)

        using (var ctx = IncrementalHash.CreateHash(HashAlgorithmName.MD5))
        {
            ctx.AppendData(pwd);
            ctx.AppendData(Utf8("$1$"));
            ctx.AppendData(saltB);
            var cnt = pwd.Length;
            while (cnt > 0)
            {
                ctx.AppendData(alt, 0, Math.Min(16, cnt));
                cnt -= 16;
            }
            cnt = pwd.Length;
            while (cnt > 0)
            {
                if ((cnt & 1) != 0) ctx.AppendData(new byte[] { 0 });
                else ctx.AppendData(pwd, 0, 1);
                cnt >>= 1;
            }
            alt = ctx.GetHashAndReset();
        }

        for (var i = 0; i < 1000; i++)
        {
            using var ctx2 = IncrementalHash.CreateHash(HashAlgorithmName.MD5);
            if ((i & 1) != 0) ctx2.AppendData(pwd);
            else ctx2.AppendData(alt);
            if (i % 3 != 0) ctx2.AppendData(saltB);
            if (i % 7 != 0) ctx2.AppendData(pwd);
            if ((i & 1) != 0) ctx2.AppendData(alt);
            else ctx2.AppendData(pwd);
            alt = ctx2.GetHashAndReset();
        }

        var groups = new (int a, int b, int c)[] { (0, 6, 12), (1, 7, 13), (2, 8, 14), (3, 9, 15), (4, 10, 5), (11, -1, -1) };
        var sb = new StringBuilder("$1$").Append(salt).Append('$');
        foreach (var g in groups)
        {
            if (g.b < 0)
            {
                uint v = alt[g.a];
                sb.Append(Char64(v)).Append(Char64(v >> 6));
            }
            else
            {
                uint v = (uint)((alt[g.a] << 16) | (alt[g.b] << 8) | alt[g.c]);
                sb.Append(Char64(v)).Append(Char64(v >> 6)).Append(Char64(v >> 12)).Append(Char64(v >> 18));
            }
        }
        return sb.ToString();
    }

    // ===================== SHA-256-crypt ($5$) =====================
    private static string SHA256Crypt(string password, string stored)
    {
        var (salt, rounds) = ExtraerSalYRounds(stored, 3, 16); // "$5$" son 3 caracteres
        var pwd = Utf8(password);
        var saltB = Utf8(salt);
        const int D = 32;

        var alt = SHA256.HashData(Concat(pwd, saltB, pwd));

        using (var ctx = IncrementalHash.CreateHash(HashAlgorithmName.SHA256))
        {
            ctx.AppendData(pwd);
            ctx.AppendData(saltB);
            var cnt = pwd.Length;
            while (cnt > D) { ctx.AppendData(alt); cnt -= D; }
            ctx.AppendData(alt, 0, cnt);
            cnt = pwd.Length;
            while (cnt > 0)
            {
                if ((cnt & 1) != 0) ctx.AppendData(alt);
                else ctx.AppendData(pwd);
                cnt >>= 1;
            }
            alt = ctx.GetHashAndReset();
        }

        var pDigest = SHA256.HashData(Repeat(pwd, pwd.Length));
        var pBytes = Replicate(pDigest, pwd.Length);
        var sDigest = SHA256.HashData(Repeat(saltB, 16 + alt[0]));
        var sBytes = Replicate(sDigest, saltB.Length);

        for (var cnt = 0; cnt < rounds; cnt++)
        {
            using var c = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
            if ((cnt & 1) != 0) c.AppendData(pBytes);
            else c.AppendData(alt);
            if (cnt % 3 != 0) c.AppendData(sBytes);
            if (cnt % 7 != 0) c.AppendData(pBytes);
            if ((cnt & 1) != 0) c.AppendData(alt);
            else c.AppendData(pBytes);
            alt = c.GetHashAndReset();
        }

        var groups = new (int a, int b, int c, int n)[] {
            (0,10,20,4),(21,1,11,4),(12,22,2,4),(3,13,23,4),(24,4,14,4),
            (15,25,5,4),(6,16,26,4),(27,7,17,4),(18,28,8,4),(9,19,29,4),
            (-1,31,30,3) };
        var sb = new StringBuilder("$5$");
        if (rounds != 5000) sb.Append("rounds=").Append(rounds).Append('$');
        sb.Append(salt).Append('$');
        foreach (var g in groups)
        {
            uint v;
            var n = g.n;
            if (g.a < 0) v = (uint)((alt[g.b] << 8) | alt[g.c]);
            else v = (uint)((alt[g.a] << 16) | (alt[g.b] << 8) | alt[g.c]);
            while (n-- > 0) { sb.Append(Char64(v)); v >>= 6; }
        }
        return sb.ToString();
    }

    // ===================== SHA-512-crypt ($6$) =====================
    private static string SHA512Crypt(string password, string stored)
    {
        var (salt, rounds) = ExtraerSalYRounds(stored, 3, 16); // "$6$" son 3 caracteres
        var pwd = Utf8(password);
        var saltB = Utf8(salt);
        const int D = 64;

        var alt = SHA512.HashData(Concat(pwd, saltB, pwd));

        using (var ctx = IncrementalHash.CreateHash(HashAlgorithmName.SHA512))
        {
            ctx.AppendData(pwd);
            ctx.AppendData(saltB);
            var cnt = pwd.Length;
            while (cnt > D) { ctx.AppendData(alt); cnt -= D; }
            ctx.AppendData(alt, 0, cnt);
            cnt = pwd.Length;
            while (cnt > 0)
            {
                if ((cnt & 1) != 0) ctx.AppendData(alt);
                else ctx.AppendData(pwd);
                cnt >>= 1;
            }
            alt = ctx.GetHashAndReset();
        }

        var pDigest = SHA512.HashData(Repeat(pwd, pwd.Length));
        var pBytes = Replicate(pDigest, pwd.Length);
        var sDigest = SHA512.HashData(Repeat(saltB, 16 + alt[0]));
        var sBytes = Replicate(sDigest, saltB.Length);

        for (var cnt = 0; cnt < rounds; cnt++)
        {
            using var c = IncrementalHash.CreateHash(HashAlgorithmName.SHA512);
            if ((cnt & 1) != 0) c.AppendData(pBytes);
            else c.AppendData(alt);
            if (cnt % 3 != 0) c.AppendData(sBytes);
            if (cnt % 7 != 0) c.AppendData(pBytes);
            if ((cnt & 1) != 0) c.AppendData(alt);
            else c.AppendData(pBytes);
            alt = c.GetHashAndReset();
        }

        var groups = new (int a, int b, int c)[] {
            (0,21,42),(22,43,1),(44,2,23),(3,24,45),(25,46,4),(47,5,26),
            (6,27,48),(28,49,7),(50,8,29),(9,30,51),(31,52,10),(53,11,32),
            (12,33,54),(34,55,13),(56,14,35),(15,36,57),(37,58,16),(59,17,38),
            (18,39,60),(40,61,19),(62,20,41),(63,-1,-1) };
        var sb = new StringBuilder("$6$");
        if (rounds != 5000) sb.Append("rounds=").Append(rounds).Append('$');
        sb.Append(salt).Append('$');
        foreach (var g in groups)
        {
            if (g.b < 0)
            {
                var b = alt[g.a];
                sb.Append(Char64((uint)(b & 0x3f))).Append(Char64((uint)b >> 6));
            }
            else
            {
                uint v = (uint)((alt[g.a] << 16) | (alt[g.b] << 8) | alt[g.c]);
                sb.Append(Char64(v)).Append(Char64(v >> 6)).Append(Char64(v >> 12)).Append(Char64(v >> 18));
            }
        }
        return sb.ToString();
    }

    // ===================== Utilidades =====================
    private static char Char64(uint v) => CryptAlphabet[(int)(v & 0x3f)];

    private static byte[] Utf8(string s) => Encoding.UTF8.GetBytes(s);

    private static byte[] Concat(params byte[][] arrays)
    {
        var total = arrays.Sum(a => a.Length);
        var result = new byte[total];
        var off = 0;
        foreach (var a in arrays) { a.CopyTo(result, off); off += a.Length; }
        return result;
    }

    private static byte[] Repeat(byte[] src, int n)
    {
        var result = new byte[src.Length * n];
        for (var i = 0; i < n; i++) src.CopyTo(result, i * src.Length);
        return result;
    }

    private static byte[] Replicate(byte[] src, int n)
    {
        var result = new byte[n];
        for (var i = 0; i < n; i++) result[i] = src[i % src.Length];
        return result;
    }

    /// <summary>Extrae la sal del hash almacenado (a partir de la posición tras "$X$").</summary>
    private static string ExtraerSal(string stored, int prefijoLong, int maxSal)
    {
        // stored = "$X$<sal>$<hash>"; la sal termina en el siguiente '$'.
        var inicio = prefijoLong; // índice tras "$X$"
        var fin = stored.IndexOf('$', inicio);
        if (fin < 0) fin = stored.Length;
        var sal = stored[inicio..fin];
        return sal.Length > maxSal ? sal[..maxSal] : sal;
    }

    /// <summary>Extrae sal y rounds para $5$/$6$ (puede llevar "rounds=N$").</summary>
    private static (string Sal, int Rounds) ExtraerSalYRounds(string stored, int prefijoLong, int maxSal)
    {
        var inicio = prefijoLong; // índice tras "$5$"/"$6$"
        var rounds = 5000;

        if (stored.Length > inicio && stored[inicio..].StartsWith("rounds=", StringComparison.Ordinal))
        {
            var idx = stored.IndexOf('$', inicio);
            if (idx > inicio)
            {
                var cadenaRounds = stored[inicio..idx];
                if (int.TryParse(cadenaRounds["rounds=".Length..], out var r))
                    rounds = r;
                inicio = idx + 1;
            }
        }

        var fin = stored.IndexOf('$', inicio);
        if (fin < 0) fin = stored.Length;
        var sal = stored[inicio..fin];
        if (sal.Length > maxSal) sal = sal[..maxSal];

        return (sal, rounds);
    }
}
