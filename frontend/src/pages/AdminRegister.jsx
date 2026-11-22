
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import apiUrl from "../api";
import "./Admin.css";

function AdminRegister() {
    const { register, authData, logout } = useAuth();
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        confirmPassword: "",
        registrationKey: ""
    });

    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);
    const [deleting, setDeleting] = useState(false);

    function handleChange(event) {
        const { name, value } = event.target;

        setFormData((previousData) => ({
            ...previousData,
            [name]: value
        }));
    }

    async function handleSubmit(event) {
        event.preventDefault();
        setError("");

        if (formData.password !== formData.confirmPassword) {
            setError("Lozinke se ne podudaraju.");
            return;
        }

        setLoading(true);

        try {
            const response = await register(formData);

            if (response.role !== "Admin") {
                throw new Error(
                    "Registracija nije kreirala Admin nalog."
                );
            }

            navigate("/admin", { replace: true });
        } catch (error) {
            setError(error.message || "Registracija nije uspela.");
        } finally {
            setLoading(false);
        }
    }

    async function handleDeleteAccount() {
        const confirmed = window.confirm(
            "Da li ste sigurni da želite trajno da obrišete administratorski nalog?"
        );

        if (!confirmed) {
            return;
        }

        try {
            setDeleting(true);
            setError("");

            const response = await fetch(
                `${apiUrl}/api/Auth/${authData.userId}`,
                {
                    method: "DELETE",
                    headers: {
                        Authorization: `Bearer ${authData.token}`
                    }
                }
            );

            if (!response.ok) {
                throw new Error(
                    "Brisanje administratorskog naloga nije uspelo."
                );
            }

            logout();

            navigate("/admin/register", { replace: true });

        } catch (error) {
            setError(error.message);
        } finally {
            setDeleting(false);
        }
    }

    return (
        <main className="admin-auth">
            <section className="admin-auth__card">
                <h1>Registracija administratora</h1>

                <p>
                    Kreiranje administratorskog naloga za DUKA SHOP.
                </p>

                {!authData ? (
                    <form onSubmit={handleSubmit}>
                        <label htmlFor="firstName">Ime</label>
                        <input
                            id="firstName"
                            name="firstName"
                            value={formData.firstName}
                            onChange={handleChange}
                            autoComplete="given-name"
                            required
                        />

                        <label htmlFor="lastName">Prezime</label>
                        <input
                            id="lastName"
                            name="lastName"
                            value={formData.lastName}
                            onChange={handleChange}
                            autoComplete="family-name"
                            required
                        />

                        <label htmlFor="register-email">Email</label>
                        <input
                            id="register-email"
                            type="email"
                            name="email"
                            value={formData.email}
                            onChange={handleChange}
                            autoComplete="email"
                            required
                        />

                        <label htmlFor="register-password">Lozinka</label>
                        <input
                            id="register-password"
                            type="password"
                            name="password"
                            value={formData.password}
                            onChange={handleChange}
                            autoComplete="new-password"
                            required
                        />

                        <label htmlFor="confirm-password">
                            Potvrdi lozinku
                        </label>
                        <input
                            id="confirm-password"
                            type="password"
                            name="confirmPassword"
                            value={formData.confirmPassword}
                            onChange={handleChange}
                            autoComplete="new-password"
                            required
                        />

                        <label htmlFor="registration-key">
                            Registracioni ključ
                        </label>
                        <input
                            id="registration-key"
                            type="password"
                            name="registrationKey"
                            value={formData.registrationKey}
                            onChange={handleChange}
                            autoComplete="off"
                            required
                        />

                        {error && (
                            <p role="alert" className="admin-auth__error">
                                {error}
                            </p>
                        )}

                        <button type="submit" disabled={loading}>
                            {loading
                                ? "Registracija..."
                                : "Registruj administratora"}
                        </button>
                    </form>
                ) : (
                    <section className="admin-auth__danger">
                        <h2>Administratorski nalog</h2>

                        <p>
                            Prijavljen administrator: {authData.email}
                        </p>

                        <p>
                            Ovde možete trajno obrisati administratorski nalog.
                        </p>

                        {error && (
                            <p role="alert" className="admin-auth__error">
                                {error}
                            </p>
                        )}

                        <button
                            type="button"
                            onClick={handleDeleteAccount}
                            disabled={deleting}
                        >
                            {deleting
                                ? "Brisanje..."
                                : "Obriši administratorski nalog"}
                        </button>
                    </section>
                )}

                <Link to="/admin/login">Već imaš nalog? Prijavi se</Link>
                <Link to="/">Nazad na prodavnicu</Link>
            </section>
        </main>
    );
}

export default AdminRegister;
