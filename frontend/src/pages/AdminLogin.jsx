
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import "./Admin.css";

function AdminLogin() {
    const { login } = useAuth();
    const navigate = useNavigate();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    async function handleSubmit(event) {
        event.preventDefault();
        setError("");
        setLoading(true);

        try {
            const response = await login(email, password);

            if (response.role !== "Admin") {
                throw new Error("Nemate administratorska prava.");
            }

            navigate("/admin", { replace: true });
        } catch (error) {
            setError(error.message || "Prijava nije uspela.");
        } finally {
            setLoading(false);
        }
    }

    return (
        <main className="admin-auth">
            <section className="admin-auth__card">
                <h1>Admin prijava</h1>
                <p>Prijavite se na administratorski panel.</p>

                <form onSubmit={handleSubmit}>
                    <label htmlFor="admin-email">Email</label>

                    <input
                        id="admin-email"
                        type="email"
                        value={email}
                        onChange={(event) => setEmail(event.target.value)}
                        autoComplete="username"
                        required
                    />

                    <label htmlFor="admin-password">Lozinka</label>

                    <input
                        id="admin-password"
                        type="password"
                        value={password}
                        onChange={(event) => setPassword(event.target.value)}
                        autoComplete="current-password"
                        required
                    />

                    {error && (
                        <p role="alert" className="admin-auth__error">
                            {error}
                        </p>
                    )}

                    <button type="submit" disabled={loading}>
                        {loading ? "Prijavljivanje..." : "Prijavi se"}
                    </button>
                </form>

                <Link to="/">Nazad na prodavnicu</Link>
            </section>
        </main>
    );
}

export default AdminLogin;
