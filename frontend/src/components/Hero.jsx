import "./Hero.css";

function Hero() {
    return (
        <section className="hero">
            <div className="hero__content">
                <span className="hero__eyebrow">
                    JEDNOSTAVNIJA KUPOVINA
                </span>

                <h1 className="hero__title">
                    Pronađite ono što vam
                    <span className="hero__title-highlight"> zaista treba.</span>
                </h1>

                <p className="hero__description">
                    Istražite ponudu proizvoda na jednom mestu.
                    Brzo pronađite kategoriju koja vas zanima i
                    pregledajte dostupne oglase.
                </p>

                <a className="hero__button" href="#marketplace">
                    Istražite ponudu
                    <span aria-hidden="true"> →</span>
                </a>

                <div className="hero__note">
                    <span className="hero__note-dot" />
                    Pregledajte ponudu bez registracije
                </div>
            </div>

            <div className="hero__visual" aria-hidden="true">
                <div className="hero__visual-glow" />

                <div className="hero__visual-card hero__visual-card--back">
                    <span className="hero__visual-line" />
                    <span className="hero__visual-line hero__visual-line--short" />
                    <span className="hero__visual-block" />
                </div>

                <div className="hero__visual-card hero__visual-card--front">
                    <div className="hero__visual-top">
                        <span className="hero__visual-symbol">W</span>
                        <span className="hero__visual-status" />
                    </div>

                    <div className="hero__visual-art">
                        <div className="hero__visual-orb" />
                        <div className="hero__visual-ring" />
                        <div className="hero__visual-cube" />
                    </div>

                    <div className="hero__visual-bottom">
                        <span className="hero__visual-line" />
                        <span className="hero__visual-line hero__visual-line--short" />
                    </div>
                </div>

                <div className="hero__floating-badge">
                    <span className="hero__floating-check">✓</span>
                    <span>
                        <strong>Jednostavno</strong>
                        <small>Pretraživanje ponude</small>
                    </span>
                </div>
            </div>
        </section>
    );
}

export default Hero;