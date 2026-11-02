
import { useState } from "react";
import apiUrl from "../api";
import "./EditProduct.css";

const STATUSES = ["ACTIVE", "INACTIVE", "SOLD"];

function EditProduct({ product, categories, onSaved, onClose }) {
    const [name, setName] = useState(product.name ?? "");
    const [description, setDescription] = useState(product.description ?? "");
    const [price, setPrice] = useState(String(product.price ?? ""));
    const [quantity, setQuantity] = useState(String(product.quantity ?? 1));
    const [status, setStatus] = useState(
        STATUSES.includes(product.status) ? product.status : ""
    );
    const [categoryId, setCategoryId] = useState(String(product.categoryId ?? ""));
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    async function handleSubmit(event) {
        event.preventDefault();
        setError("");

        if (name.trim().length < 2) {
            setError("Naziv mora imati najmanje 2 karaktera.");
            return;
        }
        if (description.trim().length < 2) {
            setError("Opis mora imati najmanje 2 karaktera.");
            return;
        }
        if (price === "" || !Number.isFinite(Number(price)) || Number(price) < 0) {
            setError("Unesi ispravnu cenu.");
            return;
        }
        if (quantity === "" || !Number.isInteger(Number(quantity)) || Number(quantity) < 0) {
            setError("Količina mora biti ceo broj, najmanje 0.");
            return;
        }
        if (!STATUSES.includes(status)) {
            setError("Izaberi status proizvoda.");
            return;
        }
        if (!categoryId) {
            setError("Izaberi kategoriju proizvoda.");
            return;
        }

        setLoading(true);

        try {
            const response = await fetch(`${apiUrl}/api/Product/${product.id}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    name: name.trim(),
                    description: description.trim(),
                    price: Number(price),
                    quantity: Number(quantity),
                    status: status,
                    categoryId: Number(categoryId)
                })
            });

            if (!response.ok) {
                let message = `Greška pri izmeni proizvoda: ${response.status}`;

                try {
                    const data = await response.json();
                    message = data?.message || data?.title || message;
                } catch {
                    // odgovor nije JSON
                }

                throw new Error(message);
            }

            onSaved();
        } catch (requestError) {
            setError(requestError.message || "Došlo je do greške.");
        } finally {
            setLoading(false);
        }
    }

    return (
        <div
            className="edit-product__overlay"
            onMouseDown={(event) => {
                if (event.target === event.currentTarget && !loading) {
                    onClose();
                }
            }}
            onClick={(event) => event.stopPropagation()}
        >
            <section
                className="edit-product"
                role="dialog"
                aria-modal="true"
                aria-labelledby="edit-product-title"
            >
                <div className="edit-product__header">
                    <h2 id="edit-product-title">Izmeni proizvod</h2>

                    <button
                        type="button"
                        className="edit-product__close"
                        onClick={onClose}
                        disabled={loading}
                        aria-label="Zatvori formu"
                    >
                        ×
                    </button>
                </div>

                <form onSubmit={handleSubmit}>
                    <label htmlFor="edit-name">Naziv proizvoda</label>
                    <input
                        id="edit-name"
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        minLength={2}
                        maxLength={150}
                        required
                    />

                    <label htmlFor="edit-description">Opis proizvoda</label>
                    <textarea
                        id="edit-description"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        minLength={2}
                        maxLength={2000}
                        rows={5}
                        required
                    />

                    <div className="edit-product__row">
                        <div>
                            <label htmlFor="edit-price">Cena</label>
                            <input
                                id="edit-price"
                                type="number"
                                min="0"
                                step="0.01"
                                value={price}
                                onChange={(e) => setPrice(e.target.value)}
                                required
                            />
                        </div>

                        <div>
                            <label htmlFor="edit-quantity">Količina</label>
                            <input
                                id="edit-quantity"
                                type="number"
                                min="0"
                                step="1"
                                value={quantity}
                                onChange={(e) => setQuantity(e.target.value)}
                                required
                            />
                        </div>
                    </div>

                    <label htmlFor="edit-status">Status</label>
                    <select
                        id="edit-status"
                        value={status}
                        onChange={(e) => setStatus(e.target.value)}
                        required
                    >
                        <option value="">Izaberi status</option>

                        {STATUSES.map((s) => (
                            <option key={s} value={s}>
                                {s}
                            </option>
                        ))}
                    </select>

                    <label htmlFor="edit-category">Kategorija</label>
                    <select
                        id="edit-category"
                        value={categoryId}
                        onChange={(e) => setCategoryId(e.target.value)}
                        required
                    >
                        <option value="">Izaberi kategoriju</option>

                        {categories.map((category) => (
                            <option key={category.id} value={category.id}>
                                {category.name}
                            </option>
                        ))}
                    </select>

                    {error && (
                        <p className="edit-product__error" role="alert">
                            {error}
                        </p>
                    )}

                    <div className="edit-product__actions">
                        <button
                            type="button"
                            className="edit-product__cancel"
                            onClick={onClose}
                            disabled={loading}
                        >
                            Otkaži
                        </button>

                        <button
                            type="submit"
                            className="edit-product__submit"
                            disabled={loading}
                        >
                            {loading ? "Čuvanje..." : "Sačuvaj izmene"}
                        </button>
                    </div>
                </form>
            </section>
        </div>
    );
}

export default EditProduct;
