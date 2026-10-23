
import "./Footer.css";

function Footer() {
    return (
        <footer className="site-footer" id="footer">
            <div className="site-footer__glow" aria-hidden="true"></div>

            <div className="site-footer__inner">
                <div className="site-footer__main">
                    {/* BREND I OPIS */}
                    <div className="site-footer__about">
                        <a className="site-footer__brand" href="/">
                            <span className="site-footer__brand-icon">D</span>

                            <span className="site-footer__brand-name">
                                DUKA <span>SHOP</span>
                            </span>
                        </a>

                        <p className="site-footer__tagline">
                            Sve što vam treba.
                            <br />
                            <span>Na jednom mestu.</span>
                        </p>

                        <p className="site-footer__description">
                            Duka Shop — sve što vas zanima na jednom mestu!
                            Bela tehnika, telefoni, televizori, klime,
                            frižideri, zamrzivači, automobili i još mnogo toga.
                        </p>

                        <div className="site-footer__badge">
                            <span className="site-footer__badge-dot"></span>
                            Vaša kupovina, jednostavnija
                        </div>
                    </div>

                    {/* KONTAKT KARTICA */}
                    <div className="site-footer__contact-card">
                        <div className="site-footer__contact-heading">
                            <span className="site-footer__contact-icon">
                                ↗
                            </span>

                            <div>
                                <span className="site-footer__eyebrow">
                                    OSTANIMO U KONTAKTU
                                </span>

                                <h2>Kontaktirajte nas</h2>
                            </div>
                        </div>

                        <div className="site-footer__contact-item">
                            <span className="site-footer__label">
                                KONTAKT OSOBA
                            </span>

                            <strong>Dušan Kukić</strong>
                        </div>

                        <div className="site-footer__contact-item">
                            <span className="site-footer__label">
                                NAZIV FIRME
                            </span>

                            <strong>Duka Shop</strong>
                        </div>

                        <div className="site-footer__phone">
                            <div className="site-footer__phone-info">
                                <span className="site-footer__phone-icon">
                                    ☎
                                </span>

                                <div>
                                    <span className="site-footer__label">
                                        POZOVITE NAS
                                    </span>

                                    <a href="tel:+381641360754">
                                        064 136 0754
                                    </a>
                                </div>
                            </div>

                            <a
                                className="site-footer__call-button"
                                href="tel:+381641360754"
                                aria-label="Pozovite Duka Shop"
                            >
                                →
                            </a>
                        </div>
                    </div>
                </div>

                {/* DONJA TRAKA */}
                <div className="site-footer__bottom">
                    <p>
                        © {new Date().getFullYear()}{" "}
                        <strong>DUKA SHOP</strong>. Sva prava zadržana.
                    </p>

                    <a className="site-footer__back-to-top" href="#root">
                        Nazad na vrh <span>↑</span>
                    </a>
                </div>
            </div>
        </footer>
    );
}

export default Footer;
