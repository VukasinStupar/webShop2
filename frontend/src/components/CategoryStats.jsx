import "./CategoryStats.css";

function CategoryStats({ stats }) {
    // stats = [{ name: "Laptopovi", count: 24 }, ...]
    if (!stats || stats.length === 0) return null;

    const max = Math.max(...stats.map((s) => s.count), 1);

    const sorted = [...stats].sort((a, b) => b.count - a.count);

    return (
        <section className="category-stats">
            <h2 className="category-stats__title">Statistika po kategorijama</h2>

            <div className="category-stats__list">
                {sorted.map((item) => {
                    const percent = Math.round((item.count / max) * 100);

                    return (
                        <div className="category-stats__row" key={item.name}>
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
                                    style={{ width: `${percent}%` }}
                                />
                            </div>

                            <span className="category-stats__count">
                                {item.count} kom
                            </span>
                        </div>
                    );
                })}
            </div>
        </section>
    );
}

export default CategoryStats;