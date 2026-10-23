
import { useEffect, useState } from "react";
import apiUrl from "../api";
import "./CreateProduct.css";

const MAX_IMAGES = 15;

function CreateProduct({ categories, onCreated, onClose }) {
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [price, setPrice] = useState("");
    const [quantity, setQuantity] = useState("");
    const [categoryId, setCategoryId] = useState("");
    const [images, setImages] = useState([]);
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        return () => {
            images.forEach((image) => {
                URL.revokeObjectURL(image.previewUrl);
            });
        };
    }, [images]);

    function handleImagesChange(event) {
        const selectedFiles = Array.from(event.target.files || []);

        // Omogućava da se isti fajl ponovo izabere.
        event.target.value = "";

        const validFiles = selectedFiles.filter((file) =>
            file.type.startsWith("image/")
        );

        if (validFiles.length !== selectedFiles.length) {
            setError("Možeš dodavati samo fajlove slika.");
        } else {
            setError("");
        }

        const remainingSlots = MAX_IMAGES - images.length;

        if (validFiles.length > remainingSlots) {
            setError(
                `Možeš dodati najviše ${MAX_IMAGES} slika ukupno.`
            );
        }

        const filesToAdd = validFiles.slice(0, remainingSlots);

        setImages((currentImages) => [
            ...currentImages,
            ...filesToAdd.map((file) => ({
                file,
                previewUrl: URL.createObjectURL(file)
            }))
        ]);
    }

    function removeImage(indexToRemove) {
        setImages((currentImages) =>
            currentImages.filter((_, index) => index !== indexToRemove)
        );
    }

    async function handleSubmit(event) {
        event.preventDefault();
        setError("");

        if (!name.trim() || name.trim().length < 2) {
            setError("Naziv mora imati najmanje 2 karaktera.");
            return;
        }

        if (
            !description.trim() ||
            description.trim().length < 2
        ) {
            setError("Opis mora imati najmanje 2 karaktera.");
            return;
        }

        if (price === "" || !Number.isFinite(Number(price)) ||
            Number(price) < 0) {
            setError("Unesi ispravnu cenu.");
            return;
        }

        if (
            quantity === "" ||
            !Number.isInteger(Number(quantity)) ||
            Number(quantity) < 0
        ) {
            setError("Količina mora biti ceo broj, najmanje 0.");
            return;
        }

        if (!categoryId || Number(categoryId) < 1) {
            setError("Izaberi kategoriju proizvoda.");
            return;
        }

        const formData = new FormData();

        formData.append("Name", name.trim());
        formData.append("Description", description.trim());
        formData.append("Price", price);
        formData.append("Quantity", quantity);
        formData.append("CategoryId", categoryId);

        images.forEach((image) => {
            formData.append("Images", image.file);
        });

        setLoading(true);

        try {
            const response = await fetch(`${apiUrl}/api/Product`, {
                method: "POST",
                body: formData
            });

            const responseText = await response.text();
            let responseData = null;

            try {
                responseData = responseText
                    ? JSON.parse(responseText)
                    : null;
            } catch {
                responseData = null;
            }

            if (!response.ok) {
                throw new Error(
                    responseData?.message ||
                    responseData?.title ||
                    `Greška pri kreiranju proizvoda: ${response.status}`
                );
            }

            onCreated(responseData);
        } catch (requestError) {
            setError(
                requestError.message ||
                "Došlo je do greške prilikom kreiranja proizvoda."
            );
        } finally {
            setLoading(false);
        }
    }

    return (
        <div
            className="create-product__overlay"
            onMouseDown={(event) => {
                if (event.target === event.currentTarget && !loading) {
                    onClose();
                }
            }}
        >
            <section
                className="create-product"
                role="dialog"
                aria-modal="true"
                aria-labelledby="create-product-title"
            >
                <div className="create-product__header">
                    <h2 id="create-product-title">
                        Dodaj proizvod
                    </h2>

                    <button
                        type="button"
                        className="create-product__close"
                        onClick={onClose}
                        disabled={loading}
                        aria-label="Zatvori formu"
                    >
                        ×
                    </button>
                </div>

                <form onSubmit={handleSubmit}>
                    <label htmlFor="product-name">
                        Naziv proizvoda
                    </label>
                    <input
                        id="product-name"
                        type="text"
                        value={name}
                        onChange={(event) => setName(event.target.value)}
                        minLength={2}
                        maxLength={150}
                        required
                    />

                    <label htmlFor="product-description">
                        Opis proizvoda
                    </label>
                    <textarea
                        id="product-description"
                        value={description}
                        onChange={(event) =>
                            setDescription(event.target.value)
                        }
                        minLength={2}
                        maxLength={2000}
                        rows={5}
                        required
                    />

                    <div className="create-product__row">
                        <div>
                            <label htmlFor="product-price">
                                Cena
                            </label>
                            <input
                                id="product-price"
                                type="number"
                                min="0"
                                step="0.01"
                                value={price}
                                onChange={(event) =>
                                    setPrice(event.target.value)
                                }
                                required
                            />
                        </div>

                        <div>
                            <label htmlFor="product-quantity">
                                Količina
                            </label>
                            <input
                                id="product-quantity"
                                type="number"
                                min="0"
                                step="1"
                                value={quantity}
                                onChange={(event) =>
                                    setQuantity(event.target.value)
                                }
                                required
                            />
                        </div>
                    </div>

                    <label htmlFor="product-category">
                        Kategorija
                    </label>
                    <select
                        id="product-category"
                        value={categoryId}
                        onChange={(event) =>
                            setCategoryId(event.target.value)
                        }
                        required
                    >
                        <option value="">
                            Izaberi kategoriju
                        </option>

                        {categories.map((category) => (
                            <option
                                key={category.id}
                                value={category.id}
                            >
                                {category.name}
                            </option>
                        ))}
                    </select>

                    <div className="create-product__image-heading">
                        <label htmlFor="product-images">
                            Slike proizvoda
                        </label>
                        <span>
                            {images.length} / {MAX_IMAGES}
                        </span>
                    </div>

                    <input
                        id="product-images"
                        type="file"
                        accept="image/*"
                        multiple
                        disabled={images.length >= MAX_IMAGES}
                        onChange={handleImagesChange}
                    />

                    <p className="create-product__hint">
                        Izaberi jednu sliku ili više slika odjednom.
                        Slike nisu obavezne.
                    </p>

                    {images.length > 0 && (
                        <div className="create-product__images">
                            {images.map((image, index) => (
                                <div
                                    className="create-product__image"
                                    key={image.previewUrl}
                                >
                                    <img
                                        src={image.previewUrl}
                                        alt={`Pregled slike ${index + 1}`}
                                    />

                                    <button
                                        type="button"
                                        onClick={() => removeImage(index)}
                                        disabled={loading}
                                        aria-label={`Ukloni sliku ${index + 1}`}
                                    >
                                        ×
                                    </button>
                                </div>
                            ))}
                        </div>
                    )}

                    {error && (
                        <p
                            className="create-product__error"
                            role="alert"
                        >
                            {error}
                        </p>
                    )}

                    <div className="create-product__actions">
                        <button
                            type="button"
                            className="create-product__cancel"
                            onClick={onClose}
                            disabled={loading}
                        >
                            Otkaži
                        </button>

                        <button
                            type="submit"
                            className="create-product__submit"
                            disabled={loading}
                        >
                            {loading
                                ? "Kreiranje..."
                                : "Kreiraj proizvod"}
                        </button>
                    </div>
                </form>
            </section>
        </div>
    );
}

export default CreateProduct;
