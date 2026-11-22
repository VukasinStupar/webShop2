
import "./CategoryStats.css";
import { useAuth } from "../context/AuthContext";
import apiUrl from "../api";
import { useEffect, useState } from "react";

function CategoryStats() {
    const { token, isAdmin } = useAuth();

    const [stats, setStats] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    // Učitavanje statistike sa backend-a
    useEffect(() => {
        const loadStatistics = async () => {
            if (!isAdmin || !token) {
                setLoading(false);
                return;
            }

            try {
                setLoading(true);
                setError("");

                const response = await fetch(
                    `${apiUrl}/api/Category/statistics`,
                    {
                        method: "GET",
                        headers: {
                            "Authorization": `Bearer ${token}`,
                            "Content-Type": "application/json"
                        }
                    }
                );


                if (!response.ok) {
                    throw new Error("Failed to load category statistics.");
                }

                const data = await response.json();
                console.log("Category statistics:", data);
                setStats(data);
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        };

        loadStatistics();
    }, [isAdmin, token]);

    // Samo administrator može da vidi statistiku
    if (!isAdmin || !token) {
        return null;
    }

    if (loading) {
        return <p>Učitavanje statistike...</p>;
    }

    if (error) {
        return <p>Greška: {error}</p>;
    }

    if (!stats || stats.length === 0) {
        return <p>Nema dostupnih statističkih podataka.</p>;
    }

    // Pronalazimo najveći broj proizvoda u kategoriji
    const max = Math.max(...stats.map((item) => item.count), 1);

    // Sortiramo kategorije od najveće do najmanje
    const sorted = [...stats].sort(
        (a, b) => b.count - a.count
    );

    // Formatiranje cene u evrima
    const formatPrice = (price) => {
        return new Intl.NumberFormat("sr-RS", {
            style: "currency",
            currency: "EUR",
            maximumFractionDigits: 2
        }).format(price ?? 0);
    };

    // Ukupna vrednost svih kategorija
    const totalPrice = stats.reduce(
        (sum, item) => sum + (item.totalPrice ?? 0),
        0
    );

    // Ukupan broj svih artikala u svim kategorijama
    const totalCount = stats.reduce(
        (sum, item) => sum + (item.count ?? 0),
        0
    );

    return (
        <section className="category-stats">

            <h2 className="category-stats__title">
                Statistika po kategorijama
            </h2>

            <div className="category-stats__list">

                {sorted.map((item) => {
                    const percent = Math.round(
                        (item.count / max) * 100
                    );

                    return (
                        <div
                            className="category-stats__row"
                            key={item.categoryId}
                        >
                            <span className="category-stats__name">
                                {item.name}
                            </span>

                            <div
                                className="category-stats__bar"
                                role="progressbar"
                                aria-valuenow={percent}
                                aria-valuemin={0}
                                aria-valuemax={100}
                            >
                                <div
                                    className="category-stats__fill"
                                    style={{
                                        width: `${percent}%`
                                    }}
                                />
                            </div>

                            <span className="category-stats__count">
                                {item.count} kom
                            </span>

                            <span className="category-stats__price">
                                {formatPrice(item.totalPrice)}
                            </span>
                        </div>
                    );
                })}

            </div>

            <div className="category-stats__total">
                <span>
                    Ukupan broj svih artikala
                </span>

                <strong>
                    {totalCount} kom
                </strong>

                <span>
                    Ukupna vrednost svih kategorija
                </span>

                <strong>
                    {formatPrice(totalPrice)}
                </strong>
            </div>

        </section>
    );
}

export default CategoryStats;