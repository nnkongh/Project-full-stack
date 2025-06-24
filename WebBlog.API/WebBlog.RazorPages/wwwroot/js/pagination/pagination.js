

export function renderPagination(pageIndex, totalPages, onPageChange) {
    const container = document.getElementById("pagination");
    container.innerHTML = "";

    if (pageIndex > 1) {
        const prevBtn = createButton("Trước", () => onPageChange(pageIndex - 1)); // onPageChange là hàm callback để xử lý sự kiện khi chuyển trang 
        container.appendChild(prevBtn);
    }

    const info = document.createElement("span");
    info.textContent = `Trang ${pageIndex} / ${totalPages}`;
    container.appendChild(info);

    if (pageIndex < totalPages) {
        const nextBtn = createButton("Sau", () => onPageChange(pageIndex + 1));
        container.appendChild(nextBtn);
    }
}

function createButton(text, onClick) {
    const btn = document.createElement("button");
    btn.textContent = text;
    btn.onclick = onClick;
    return btn;
}
