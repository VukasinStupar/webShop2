
import { useState } from "react";
import { useAuth } from "../context/AuthContext";
import { useNavigate, Link } from "react-router-dom";
import apiUrl from "../api";
import "./Admin.css";

function AdminDashboard() {
    const { authData, logout } = useAuth();
    const navigate = useNavigate();

    const [deleting, setDeleting] = useState(false);
    const [error, setError] = useState("");

    function handleLogout() {
        logout();
        navigate("/admin/login", { replace: true });
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
                `${apiUrl}/api/Auth/delete-account`,
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
        <main className="admin-dashboard">

            <header className="admin-dashboard__header">
                <h1>DUKA SHOP – Administracija</h1>

                <button onClick={handleLogout}>
                    Odjavi se
                </button>
            </header>

            <section>
                <h2>Dobrodošao, {authData.firstName}!</h2>

                <p>
                    Prijavljen administrator: {authData.email}
                </p>

                <p>
                    Uloga: {authData.role}
                </p>
            </section>

            <section className="admin-dashboard__actions">
                <h2>Upravljanje prodavnicom</h2>

                <Link to="/">
                    Otvori prodavnicu
                </Link>
            </section>

            <section className="admin-dashboard__danger">
                <h2>Opasna zona</h2>

                <p>
                    Trajno brisanje administratorskog naloga.
                    Nakon brisanja bićete automatski odjavljeni.
                </p>

                {error && (
                    <p className="admin-dashboard__error">
                        {error}
                    </p>
                )}

                <button
                    className="admin-dashboard__delete"
                    onClick={handleDeleteAccount}
                    disabled={deleting}
                >
                    {deleting
                        ? "Brisanje..."
                        : "Obriši administratorski nalog"}
                </button>
            </section>

        </main>
    );
}

export default AdminDashboard;
