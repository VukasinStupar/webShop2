import { useEffect, useRef, useState } from "react";
import { getMainImage, sortImages } from "../utils/images";
import apiUrl from "../api";
import "./ProductProfile.css";

const IMAGE_EXTENSIONS =
    /\.(jpe?g|jfif|png|gif|webp|bmp|tiff?|heic|heif|avif|svg)$/i;
const MAX_SIZE = 10_000_000; // Cloudinary besplatni plan: najviše 10 MB po slici

// DODATO: kontakt podaci na jednom mestu
const PHONE_DISPLAY = "064 136 0754";
const PHONE_HREF = "tel:+381641360754";

function isImageFile(file) {
    return file.type.startsWith("image/") || IMAGE_EXTENSIONS.test(file.name);
}

// DODATO: telefon / tablet (ekran na dodir)
function isTouchDevice() {
    return (
        window.matchMedia?.("(pointer: coarse)").matches ||
        /Android|iPhone|iPad|iPod/i.test(navigator.userAgent)
    );
}

function ProductProfile({ product, onClose, onChanged, isAdmin = false }) {
    const images = sortImages(product);

    const [selectedId, setSelectedId] = useState(null);
    const [busy, setBusy] = useState(false);
    const [progress, setProgress] = useState(null);
    const [message, setMessage] = useState("");

    // DODATO
    const [showPhone, setShowPhone] = useState(false);
    const [copied, setCopied] = useState(false);

    const fileInputRef = useRef(null);

    const selected =
        images.find((img) => img.id === selectedId) ?? getMainImage(product);

    useEffect(() => {
        function onKeyDown(e) {
            if (e.key === "Escape") onClose();
        }
        window.addEventListener("keydown", onKeyDown);
        return () => window.removeEventListener("keydown", onKeyDown);
    }, [onClose]);

    // DODATO: klik na dugme za poziv
    function handleCallClick() {
        if (isTouchDevice()) {
            // telefon: odmah otvori biranje broja
            window.location.href = PHONE_HREF;
            return;
        }

        // računar: prikaži broj sa opcijama
        setShowPhone((prev) => !prev);
        setCopied(false);
    }

    // DODATO: kopiranje broja
    async function handleCopyPhone() {
        try {
            await navigator.clipboard.writeText(PHONE_DISPLAY);
        } catch {
            // rezervno rešenje ako clipboard nije dostupan
            const input = document.createElement("input");
            input.value = PHONE_DISPLAY;
            document.body.appendChild(input);
            input.select();
            document.execCommand("copy");
            document.body.removeChild(input);
        }

        setCopied(true);
        setTimeout(() => setCopied(false), 2000);
    }

    async function runAction(action) {
        try {
            setBusy(true);
            setMessage("");
            await action();
            await onChanged?.();
        } catch (err) {
            setMessage(err.message || "Akcija nije uspela.");
        } finally {
            setBusy(false);
        }
    }

    function handleSetMain() {
        if (!selected) return;

        runAction(async () => {
            const res = await fetch(`${apiUrl}/api/image/${selected.id}/main`, {
                method: "PUT"
            });
            if (!res.ok) throw new Error("Postavljanje profilne slike nije uspelo.");
        });
    }

    function handleDelete() {
        if (!selected) return;

        if (!window.confirm("Da li sigurno želiš da obrišeš ovu sliku sa sajta?")) {
            return;
        }

        runAction(async () => {
            const res = await fetch(`${apiUrl}/api/image/${selected.id}`, {
                method: "DELETE"
            });
            if (!res.ok) throw new Error("Brisanje slike nije uspelo.");
            setSelectedId(null);
        });
    }

    // Upload više slika odjednom: šalje se jedna po jedna, redom
    async function handleUpload(e) {
        const files = Array.from(e.target.files ?? []);
        e.target.value = "";

        if (files.length === 0) return;

        setBusy(true);
        setMessage("");

        const failed = [];

        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            setProgress({ current: i + 1, total: files.length });

            if (!isImageFile(file)) {
                failed.push(`${file.name} (nije slika)`);
                continue;
            }

            if (file.size > MAX_SIZE) {
                failed.push(`${file.name} (veće od 10 MB)`);
                continue;
            }

            try {
                const formData = new FormData();
                formData.append("file", file);
                formData.append("isMain", false);

                const res = await fetch(
                    `${apiUrl}/api/image/upload/${product.id}`,
                    { method: "POST", body: formData }
                );

                if (!res.ok) {
                    failed.push(file.name);
                }
            } catch {
                failed.push(file.name);
            }
        }

        setProgress(null);

        // lista se osvežava jednom, kad se sve pošalje
        await onChanged?.();
        setBusy(false);

        if (failed.length > 0) {
            setMessage(
                `Nije uspelo ${failed.length} od ${files.length}: ${failed.join(", ")}`
            );
        }
    }

    return (
        <div className="product-profile__backdrop" onClick={onClose}>
            <div
                className="product-profile"
                onClick={(e) => e.stopPropagation()}
            >
                <button
                    className="product-profile__close"
                    onClick={onClose}
                    aria-label="Zatvori"
                >
                    ×
                </button>

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

                    {/* DUGMAD ZA IZABRANU SLIKU */}
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
                                        alt=""
                                        className={
                                            "product-profile__thumb" +
                                            (selected?.id === img.id
                                                ? " product-profile__thumb--active"
                                                : "")
                                        }
                                        onClick={() => setSelectedId(img.id)}
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
                                onClick={() => fileInputRef.current?.click()}
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
                        <p className="product-profile__error">{message}</p>
                    )}
                </div>

                <div className="product-profile__info">
                    <h2>{product.name}</h2>

                    <p className="product-profile__price">
                        {product.price == null
                            ? "Cena nije navedena"
                            : `${Number(product.price).toLocaleString("sr-RS")} RSD`}
                    </p>

                    <p>{product.description}</p>

                    <p>Dostupna količina: {product.quantity}</p>
                    <p>Status: {product.status}</p>

                    <div className="product-profile__contact">
                        <h3>Kontakt</h3>
                        <p>
                            <strong>Ime i prezime:</strong> Dušan Kukić
                        </p>
                        <p>
                            <strong>Firma:</strong> Duka Shop
                        </p>

                        {/* IZMENJENO: dugme umesto običnog linka */}
                        <button
                            type="button"
                            className="product-profile__call"
                            onClick={handleCallClick}
                        >
                            Pozovite: {PHONE_DISPLAY}
                        </button>

                        {/* DODATO: okvir za računar */}
                        {showPhone && (
                            <div className="product-profile__phone-box">
                                <span className="product-profile__phone-number">
                                    {PHONE_DISPLAY}
                                </span>

                                <div className="product-profile__phone-actions">
                                    <a href={PHONE_HREF}>Pozovi</a>

                                    <button
                                        type="button"
                                        onClick={handleCopyPhone}
                                    >
                                        {copied ? "Kopirano ✓" : "Kopiraj broj"}
                                    </button>
                                </div>

                                <small>
                                    Na računaru pozivi rade samo ako imate
                                    program za pozive. Na telefonu se broj
                                    bira odmah.
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