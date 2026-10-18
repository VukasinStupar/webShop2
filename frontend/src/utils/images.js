export function sortImages(product) {
    return [...(product.images ?? [])].sort((a, b) => a.sortOrder - b.sortOrder);
}

export function getMainImage(product) {
    const images = sortImages(product);
    return images.find((i) => i.isMain) ?? images[0] ?? null;
}