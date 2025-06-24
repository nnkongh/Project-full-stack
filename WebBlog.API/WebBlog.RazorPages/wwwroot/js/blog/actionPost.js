import { remove } from "../utils/httpClient.js";

export function setupPostActions(container) {
    container.addEventListener("click", async function (e) {
        const deleteBtn = e.target.closest(".delete-btn");

        if (deleteBtn) {
            const id = deleteBtn.dataset.id;
            if (!id || !confirm("Bạn có chắc muốn xóa?")) return;

            try {
                const res = await remove(`https://localhost:7141/api/blog/delete/${id}`);
                if (res.ok) {
                    alert("Xóa thành công");
                    deleteBtn.closest(".post-box").remove();
                } else {
                    alert("Xóa thất bại");
                }
            } catch (error) {
                alert("Lỗi khi xóa bài viết: " + error.message);
            }

            return;
        }

        const editBtn = e.target.closest(".edit-btn");
        if (editBtn) {
            const id = editBtn.dataset.id;
            if (id) window.location.href = `/Blog/Edit?id=${id}`;
        }

        const viewBtn = e.target.closest(".view-btn"); 
        if (viewBtn) {
            const id = viewBtn.dataset.id;
            console.log("Click xem bài viết IDđ:", id);
            if(id) {
                window.location.href = `/Blog/Details?id=${id}`; // Chuyển hướng đến trang xem chi tiết bài viết
            }
        }
    });
}
