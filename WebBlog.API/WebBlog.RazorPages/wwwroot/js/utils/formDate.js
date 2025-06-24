export function formatDate(dateStr) {
    if (!dateStr) return "Không rõ ngày";
    const date = new Date(dateStr);
    if (isNaN(date)) return "Không rõ ngày";

    const d = date.getDate().toString().padStart(2, "0");
    const m = (date.getMonth() + 1).toString().padStart(2, "0");
    const y = date.getFullYear();
    return `${d}-${m}-${y}`;
}
