
import { useEffect, useRef, useState } from "react";
import { getMainImage, sortImages } from "../utils/images";
import apiUrl from "../api";
import "./ProductProfile.css";

const IMAGE_EXTENSIONS =
    /\.(jpe?g|jfif|png|gif|webp|bmp|tiff?|heic|heif|avif|svg)$/i;

const MAX_SIZE = 10_000_000;

const PHONE_DISPLAY = "064 136 0754";
const PHONE_HREF = "tel:+381641360754";

function isImageFile(file) {
    return (
        file.type.startsWith("image/") ||
        IMAGE_EXTENSIONS.test(file.name)
    );
}

function isTouchDevice() {
    return (
        window.matchMedia?.("(pointer: coarse)").matches ||
        /Android|iPhone|iPad|iPod/i.test(navigator.userAgent)
    );
}

async function getErrorMessage(response) {
    const responseText = await response.text();

    if (!responseText) {
        return `HTTP ${response.status}: ${response.statusText}`;
    }

    try {
        const data = JSON.parse(responseText);

        return (
            data.detail ||
            data.message ||
            data.title ||
            data.error ||
            responseText
        );
    } catch {
        return responseText;
    }
}

function ProductProfile({
    product,
    onClose,
    onChanged,
    isAdmin = false
}) {
    const images = sortImages(product);

    const [selectedId, setSelectedId] = useState(null);
    const [busy, setBusy] = useState(false);
    const [progress, setProgress] = useState(null);
    const [message, setMessage] = useState("");

    const [showPhone, setShowPhone] = useState(false);
    const [copied, setCopied] = useState(false);

    const fileInputRef = useRef(null);

    const selected =
        images.find((img) => img.id === selectedId) ??
        getMainImage(product);

    useEffect(() => {
        function handleKeyDown(event) {
            if (event.key === "Escape") {
                onClose();
            }
        }

        window.addEventListener("keydown", handleKeyDown);

        return () => {
            window.removeEventListener("keydown", handleKeyDown);
        };
    }, [onClose]);

    function handleCallClick() {
        if (isTouchDevice()) {
            window.location.href = PHONE_HREF;
            return;
        }

        setShowPhone((previous) => !previous);
        setCopied(false);
    }

    async function handleCopyPhone() {
        try {
            await navigator.clipboard.writeText(PHONE_DISPLAY);
        } catch {
            const input = document.createElement("input");

            input.value = PHONE_DISPLAY;
            document.body.appendChild(input);
            input.select();

            const copiedSuccessfully =
                document.execCommand("copy");

            document.body.removeChild(input);

            if (!copiedSuccessfully) {
                setMessage("Kopiranje broja nije uspelo.");
                return;
            }
        }

        setCopied(true);
    }

    useEffect(() => {
        if (!copied) {
            return undefined;
        }

        const timeoutId = window.setTimeout(() => {
            setCopied(false);
        }, 2000);

        return () => {
            window.clearTimeout(timeoutId);
        };
    }, [copied]);

    async function runAction(action) {
        try {
            setBusy(true);
            setMessage("");

            await action();
            await onChanged?.();
        } catch (error) {
            setMessage(
                error.message || "Akcija nije uspela."
            );
        } finally {
            setBusy(false);
        }
    }

    function handleSetMain() {
        if (!selected) {
            return;
        }

        runAction(async () => {
            const response = await fetch(
                `${apiUrl}/api/image/${selected.id}/main`,
                {
                    method: "PUT"
                }
            );

            if (!response.ok) {
                throw new Error(
                    `Postavljanje profilne slike nije uspelo: ${
                        await getErrorMessage(response)
                    }`
                );
            }
        });
    }

    function handleDelete() {
        if (!selected) {
            return;
        }

        const confirmed = window.confirm(
            "Da li sigurno želiš da obrišeš ovu sliku sa sajta?"
        );

        if (!confirmed) {
            return;
        }

        runAction(async () => {
            const response = await fetch(
                `${apiUrl}/api/image/${selected.id}`,
                {
                    method: "DELETE"
                }
            );

            if (!response.ok) {
                throw new Error(
                    `Brisanje slike nije uspelo: ${
                        await getErrorMessage(response)
                    }`
                );
            }

            setSelectedId(null);
        });
    }

    async function handleUpload(event) {
        const files = Array.from(event.target.files ?? []);

        event.target.value = "";

        if (files.length === 0) {
            return;
        }

        const invalidFiles = files
            .filter(
                (file) =>
                    !isImageFile(file) ||
                    file.size > MAX_SIZE
            )
            .map((file) => {
                if (!isImageFile(file)) {
                    return `${file.name} (nije slika)`;
                }

                return `${file.name} (veće od 10 MB)`;
            });

        const validFiles = files.filter(
            (file) =>
                isImageFile(file) &&
                file.size <= MAX_SIZE
        );

        setBusy(true);
        setMessage("");
        setProgress(null);

        try {
            const failedUploads = await validFiles.reduce(
                async (previousPromise, file, index) => {
                    const previousFailures =
                        await previousPromise;

                    setProgress({
                        current: index + 1,
                        total: validFiles.length
                    });

                    try {
                        const formData = new FormData();

                        formData.append("file", file);
                        formData.append("isMain", "false");

                        const response = await fetch(
                            `${apiUrl}/api/image/upload/${product.id}`,
                            {
                                method: "POST",
                                body: formData
                            }
                        );

                        if (!response.ok) {
                            const errorMessage =
                                await getErrorMessage(response);

                            return [
                                ...previousFailures,
                                `${file.name} (HTTP ${response.status}: ${errorMessage})`
                            ];
                        }

                        return previousFailures;
                    } catch (error) {
                        return [
                            ...previousFailures,
                            `${file.name} (${
                                error.message ||
                                "Greška u mreži"
                            })`
                        ];
                    }
                },
                Promise.resolve([])
            );

            const allFailures = [
                ...invalidFiles,
                ...failedUploads
            ];

            if (failedUploads.length < validFiles.length) {
                await onChanged?.();
            }

            if (allFailures.length > 0) {
                setMessage(
                    `Nije uspelo ${allFailures.length} od ${files.length}: ` +
                    allFailures.join(", ")
                );
            } else {
                setMessage(
                    `Uspešno je poslato ${files.length} ${
                        files.length === 1 ? "slika" : "slika"
                    }.`
                );
            }
        } catch (error) {
            setMessage(
                error.message ||
                "Došlo je do greške prilikom slanja slika."
            );
        } finally {
            setProgress(null);
            setBusy(false);
        }
    }

    return (
        <div
            className="product-profile__backdrop"
            onClick={onClose}
        >
            <div
                className="product-profile"
                onClick={(event) => event.stopPropagation()}
            >
                <button
                    type="button"
                    className="product-profile__close"
                    onClick={onClose}
                    aria-label="Zatvori"
                />

                <div className="product-profile__gallery">
                    {selected ? (
                        <div className="product-profile__image-wrap">
                            <img
                                className="product-profile__main-image"
                                src={selected.url}
                                alt={product.name}
                            />

                            {selected.isMain && (
                                <span className="product-profile__badge">
                                    Profilna
                                </span>
                            )}
                        </div>
                    ) : (
                        <div className="product-profile__no-image">
                            Nema slike
                        </div>
                    )}

                    {isAdmin && selected && (
                        <div className="product-profile__actions">
                            <button
                                type="button"
                                disabled={busy || selected.isMain}
                                onClick={handleSetMain}
                            >
                                {selected.isMain
                                    ? "Ovo je profilna slika"
                                    : "Postavi kao profilnu"}
                            </button>

                            <button
                                type="button"
                                className="product-profile__danger"
                                disabled={busy}
                                onClick={handleDelete}
                            >
                                Obriši sliku
                            </button>
                        </div>
                    )}

                    {images.length > 0 && (
                        <div className="product-profile__thumbs">
                            {images.map((img) => (
                                <div
                                    className="product-profile__thumb-item"
                                    key={img.id}
                                >
                                    <img
                                        src={img.url}
                                        alt={`${product.name} - slika`}
                                        className={
                                            "product-profile__thumb" +
                                            (
                                                selected?.id === img.id
                                                    ? " product-profile__thumb--active"
                                                    : ""
                                            )
                                        }
                                        onClick={() =>
                                            setSelectedId(img.id)
                                        }
                                    />

                                    {img.isMain && (
                                        <span className="product-profile__badge product-profile__badge--small">
                                            Profilna
                                        </span>
                                    )}
                                </div>
                            ))}
                        </div>
                    )}

                    {isAdmin && (
                        <div className="product-profile__upload">
                            <input
                                ref={fileInputRef}
                                type="file"
                                accept="image/*,.heic,.heif"
                                multiple
                                onChange={handleUpload}
                                hidden
                            />

                            <button
                                type="button"
                                disabled={busy}
                                onClick={() =>
                                    fileInputRef.current?.click()
                                }
                            >
                                {progress
                                    ? `Slanje ${progress.current}/${progress.total}...`
                                    : busy
                                        ? "Sačekaj..."
                                        : "+ Dodaj slike"}
                            </button>
                        </div>
                    )}

                    {message && (
                        <p
                            className="product-profile__error"
                            role="status"
                            aria-live="polite"
                        >
                            {message}
                        </p>
                    )}
                </div>

                <div className="product-profile__info">
                    <h2>{product.name}</h2>

                    <p className="product-profile__price">
                        {product.price == null
                            ? "Cena nije navedena"
                            : `${Number(
                                product.price
                            ).toLocaleString("sr-RS")} RSD`}
                    </p>

                    <p>{product.description}</p>

                    <p>
                        Dostupna količina: {product.quantity}
                    </p>

                    <p>Status: {product.status}</p>

                    <div className="product-profile__contact">
                        <h3>Kontakt</h3>

                        <p>
                            <strong>Ime i prezime:</strong>{" "}
                            Dušan Kukić
                        </p>

                        <p>
                            <strong>Firma:</strong> Duka Shop
                        </p>

                        <button
                            type="button"
                            className="product-profile__call"
                            onClick={handleCallClick}
                        >
                            Pozovite: {PHONE_DISPLAY}
                        </button>

                        {showPhone && (
                            <div className="product-profile__phone-box">
                                <span className="product-profile__phone-number">
                                    {PHONE_DISPLAY}
                                </span>

                                <div className="product-profile__phone-actions">
                                    <a href={PHONE_HREF}>
                                        Pozovi
                                    </a>

                                    <button
                                        type="button"
                                        onClick={handleCopyPhone}
                                    >
                                        {copied
                                            ? "Kopirano ✓"
                                            : "Kopiraj broj"}
                                    </button>
                                </div>

                                <small>
                                    Na računaru pozivi rade samo ako
                                    imate program za pozive. Na
                                    telefonu se broj bira odmah.
                                </small>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ProductProfile;
