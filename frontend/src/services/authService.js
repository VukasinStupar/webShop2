
import apiUrl from "../api";

async function sendAuthRequest(endpoint, payload) {
    const response = await fetch(`${apiUrl}/api/Auth/${endpoint}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(payload)
    });

    const data = await response.json().catch(() => null);

    if (!response.ok) {
        throw new Error(
            data?.message ||
            data?.title ||
            `Greška: ${response.status}`
        );
    }

    return data;
}

async function login(email, password) {
    return await sendAuthRequest("login", {
        email,
        password
    });
}

async function register(registerData) {
    return await sendAuthRequest("register", registerData);
}

const authService = {
    login,
    register
};

export default authService;
