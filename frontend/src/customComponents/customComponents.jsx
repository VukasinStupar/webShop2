
import "./customComponents.css";

/* ========================================
   BUTTON
======================================== */

export function CustomButton({
    children,
    className = "",
    type = "button",
    onClick,
    disabled = false,
    ariaPressed
}) {
    return (
        <button
            type={type}
            className={`custom-button ${className}`}
            onClick={onClick}
            disabled={disabled}
            aria-pressed={ariaPressed}
        >
            {children}
        </button>
    );
}

/* ========================================
   INPUT
======================================== */

export function CustomInput({
    className = "",
    type = "text",
    value,
    onChange,
    placeholder = "",
    name,
    required = false,
    disabled = false,
    min,
    max,
    step
}) {
    return (
        <input
            type={type}
            className={`custom-input ${className}`}
            value={value}
            onChange={(event) => onChange?.(event.target.value)}
            placeholder={placeholder}
            name={name}
            required={required}
            disabled={disabled}
            min={min}
            max={max}
            step={step}
        />
    );
}


/* ========================================
   TEXTAREA
======================================== */

export function CustomTextarea({
    className = "",
    value,
    onChange,
    placeholder = "",
    rows = 4,
    name,
    required = false,
    disabled = false
}) {
    return (
        <textarea
            className={`custom-textarea ${className}`}
            value={value}
            onChange={(event) => onChange?.(event.target.value)}
            placeholder={placeholder}
            rows={rows}
            name={name}
            required={required}
            disabled={disabled}
        />
    );
}


/* ========================================
   SELECT
======================================== */

export function CustomSelect({
    className = "",
    value,
    onChange,
    options = [],
    placeholder = "Izaberi opciju",
    name,
    required = false,
    disabled = false
}) {
    return (
        <select
            className={`custom-select ${className}`}
            value={value}
            onChange={(event) => onChange?.(event.target.value)}
            name={name}
            required={required}
            disabled={disabled}
        >
            <option value="">
                {placeholder}
            </option>

            {options.map((option) => (
                <option
                    key={option.value}
                    value={option.value}
                >
                    {option.label}
                </option>
            ))}
        </select>
    );
}


/* ========================================
   SEARCH
======================================== */

export function CustomSearch({
    className = "",
    value,
    onChange,
    placeholder = "Pretraži...",
    disabled = false,
    onClear
}) {
    return (
        <div className={`custom-search ${className}`}>
            <span className="custom-search__icon">
                &#128269;
            </span>

            <input
                type="search"
                value={value}
                onChange={(event) => onChange?.(event.target.value)}
                placeholder={placeholder}
                disabled={disabled}
            />

            {value && (
                <button
                    type="button"
                    className="custom-search__clear"
                    onClick={() => {
                        onChange?.("");
                        onClear?.();
                    }}
                    aria-label="Obriši pretragu"
                >
                    &times;
                </button>
            )}
        </div>
    );
}


/* ========================================
   DROPDOWN
======================================== */

export function CustomDropdown({
    children,
    className = "",
    trigger,
    disabled = false
}) {
    return (
        <details
            className={`custom-dropdown ${className}`}
        >
            <summary
                className="custom-dropdown__trigger"
                aria-disabled={disabled}
            >
                {trigger}
            </summary>

            <div className="custom-dropdown__content">
                {children}
            </div>
        </details>
    );
}


/* ========================================
   CHECKBOX
======================================== */

export function CustomCheckbox({
    checked = false,
    onChange,
    label,
    name,
    disabled = false
}) {
    return (
        <label className="custom-checkbox">
            <input
                type="checkbox"
                name={name}
                checked={checked}
                onChange={(event) => onChange?.(event.target.checked)}
                disabled={disabled}
            />

            <span className="custom-checkbox__label">
                {label}
            </span>
        </label>
    );
}


/* ========================================
   BADGE
======================================== */

export function CustomBadge({
    children,
    className = ""
}) {
    return (
        <span className={`custom-badge ${className}`}>
            {children}
        </span>
    );
}


/* ========================================
   LOADER
======================================== */

export function CustomLoader({
    text = "Učitavanje..."
}) {
    return (
        <div className="custom-loader">
            <span className="custom-loader__spinner"></span>
            <span>{text}</span>
        </div>
    );
}


/* ========================================
   EMPTY STATE
======================================== */

export function CustomEmptyState({
    title = "Nema podataka",
    description = ""
}) {
    return (
        <div className="custom-empty-state">
            <h3>{title}</h3>
            {description && <p>{description}</p>}
        </div>
    );
}
