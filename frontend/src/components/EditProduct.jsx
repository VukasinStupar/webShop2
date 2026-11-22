
import { useState } from "react";
import apiUrl from "../api";
import "./EditProduct.css";

import {
    CustomButton,
    CustomInput,
    CustomTextarea,
    CustomSelect
} from "../customComponents/customComponents";

import { useAuth } from "../context/AuthContext";

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
    const { token, isAdmin } = useAuth();

    async function handleSubmit(event) {
        event.preventDefault();
        setError("");

        if (!isAdmin || !token) {
            setError("Nemate dozvolu za ovu operaciju.");
            return;
        }

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
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                },
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

                    <CustomButton
                        type="button"
                        className="edit-product__close"
                        onClick={onClose}
                        disabled={loading}
                    >
                        ×
                    </CustomButton>
                </div>

                <form onSubmit={handleSubmit}>
                    <label htmlFor="edit-name">Naziv proizvoda</label>
                    <CustomInput
                        type="text"
                        value={name}
                        onChange={setName}
                        min={undefined}
                        max={undefined}
                        required
                    />

                    <label htmlFor="edit-description">Opis proizvoda</label>
                    <CustomTextarea
                        value={description}
                        onChange={setDescription}
                        rows={5}
                        required
                    />

                    <div className="edit-product__row">
                        <div>
                            <label htmlFor="edit-price">Cena</label>
                            <CustomInput
                                type="number"
                                min="0"
                                step="0.01"
                                value={price}
                                onChange={setPrice}
                                required
                            />
                        </div>

                        <div>
                            <label htmlFor="edit-quantity">Količina</label>
                            <CustomInput
                                type="number"
                                min="0"
                                step="1"
                                value={quantity}
                                onChange={setQuantity}
                                required
                            />
                        </div>
                    </div>

                    <label htmlFor="edit-status">Status</label>
                    <CustomSelect
                        value={status}
                        onChange={setStatus}
                        placeholder="Izaberi status"
                        options={STATUSES.map((s) => ({
                            value: s,
                            label: s
                        }))}
                        required
                    />

                    <label htmlFor="edit-category">Kategorija</label>
                    <CustomSelect
                        value={categoryId}
                        onChange={setCategoryId}
                        placeholder="Izaberi kategoriju"
                        options={categories.map((category) => ({
                            value: String(category.id),
                            label: category.name
                        }))}
                        required
                    />

                    {error && (
                        <p className="edit-product__error" role="alert">
                            {error}
                        </p>
                    )}

                    <div className="edit-product__actions">
                        <CustomButton
                            type="button"
                            className="edit-product__cancel"
                            onClick={onClose}
                            disabled={loading}
                        >
                            Otkaži
                        </CustomButton>

                        <CustomButton
                            type="submit"
                            className="edit-product__submit"
                            disabled={loading}
                        >
                            {loading ? "Čuvanje..." : "Sačuvaj izmene"}
                        </CustomButton>
                    </div>
                </form>
            </section>
        </div>
    );
}

export default EditProduct;
