// Login desde el navegador: el fetch se ejecuta en el cliente, así el navegador
// almacena la cookie de autenticación que devuelve /api/login.
window.loginApi = {
    iniciar: async function (url, login, password, recordar) {
        try {
            const res = await fetch(url, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ login: login, password: password, recordar: recordar }),
                credentials: 'same-origin'
            });
            return res.status;
        } catch (e) {
            return 0;
        }
    }
};
