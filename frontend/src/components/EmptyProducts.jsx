import "./EmptyProducts.css";

function EmptyProducts() {
    return (
        <div className="empty-products">
            <div className="empty-products__illustration" aria-hidden="true">
                <div className="empty-products__document">
                    <span className="empty-products__document-line" />
                    <span className="empty-products__document-line empty-products__document-line--short" />
                    <span className="empty-products__document-box" />
                </div>

                <span className="empty-products__search">⌕</span>
            </div>

            <h3 className="empty-products__title">
                Ponuda se priprema
            </h3>

            <p className="empty-products__description">
                Trenutno nema proizvoda za prikaz. Kada povežemo katalog sa
                serverom, ovde će se pojaviti stvarni proizvodi i njihove
                informacije.
            </p>

            <span className="empty-products__status">
                <span className="empty-products__status-dot" />
                Katalog još nije povezan
            </span>
        </div>
    );
}

export default EmptyProducts;