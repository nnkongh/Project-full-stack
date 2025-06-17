import { blogIndex, remove } from "../utils/httpClient.js";


let currentPage = 1;
const pageSize = 2;
document.addEventListener("DOMContentLoaded", async function () {
    loadBlogs(currentPage);
});
async function loadBlogs(page) {
    const container = document.getElementById("post-container");

    try {
        const res = await blogIndex("https://localhost:7141/api/blog/blogs", page, pageSize);///
        console.log("Api response:", res);
        if (!res.ok) {
            throw new Error("Không thể tải bài viết");
        }

        const resposts = await res.json();
        const posts = resposts.items || [];
        const pageIndex = resposts.index;
        const totalPages = resposts.totalPages;
        const hasPrev = pageIndex > 1;
        const hasNext = pageIndex < totalPages;

        console.log("Json response:", posts);




        container.innerHTML = "";
        posts.forEach(post => {
            console.log("Post item:", post);

            let createdAt = "Không rõ ngày";
            const dateStr = post.createdAt;

            if (dateStr) {
                const date = new Date(dateStr);
                if (!isNaN(date.getTime())) {
                    const day = date.getDate().toString().padStart(2, '0');
                    const month = (date.getMonth() + 1).toString().padStart(2, '0');
                    const year = date.getFullYear();
                    createdAt = `${day}-${month}-${year}`;
                } else {
                    console.warn("Ngày không hợp lệ:", dateStr);
                }
            }

            const postBox = document.createElement("div");
            postBox.className = "post-box";
            postBox.innerHTML = `
                <div class="header-title">
                    <h3 class="post-title">${post.title}</h3>
                    <div class="action-button">
                        <button class="btn btn-danger delete-btn" data-id="${post.id}">Xóa</button>
                        <button class="btn btn-dark edit-btn" id="editPost" data-id="${post.id}">Sửa</button>
                    </div>
                </div>
                <o><strong>${createdAt} </strong><p>
                <p>${post.content}</p>
                ${post.image ? `<img src="${post.image.replace("http://", "https://")}" alt="Image post"style="width:100%; height:1000px; max-height:550px; object-fit:contain";/>` : ""}
            `;
            container.appendChild(postBox);
        });

        renderPagination(pageIndex, totalPages, hasPrev, hasNext);


        // Sử dụng event delegation để xử lý click cho nút Xóa và Sửa
        container.addEventListener("click", async function (e) {
            const deleteBtn = e.target.closest(".delete-btn");
            if (deleteBtn) {
                const id = deleteBtn.dataset.id;
                if (!id) {
                    alert("Không tìm thấy ID");
                    return;
                }

                if (!confirm("Bạn có chắc muốn xóa?")) return;

                try {
                    const res = await remove(`https://localhost:7141/api/blog/delete/${id}`);
                    console.log(res.status);
                    if (res.ok) {
                        alert("Xóa thành công");
                        deleteBtn.closest(".post-box").remove();
                    } else {
                        alert("Xóa thất bại");
                    }
                } catch (error) {
                    alert("Lỗi khi xóa bài viết: " + error.message);
                }

                return; // tránh xử lý tiếp
            }

            const editBtn = e.target.closest(".edit-btn");
            if (editBtn) {
                const id = editBtn.dataset.id;
                if (!id) {
                    alert("Không tìm thấy ID để sửa");
                    return;
                }

                window.location.href = `/Blog/Edit?id=${id}`;
            }
        });

    } catch (error) {
        container.innerHTML = `<p class="text-danger">Lỗi: ${error.message}</p>`;
    }
    function renderPagination(pageIndex, totalPages, hasPrev, hasNext) {
        const container = document.getElementById("pagination");
        container.innerHTML = "";

        if (hasPrev) {
            const prevBtn = document.createElement("button");
            prevBtn.textContent = "Trước";
            prevBtn.onclick = () => loadBlogs(pageIndex - 1);
            container.appendChild(prevBtn);
        }
        const pageInfo = document.createElement("span");
        pageInfo.textContent = `Trang ${pageIndex} / ${totalPages}`;
        container.appendChild(pageInfo);

        if (hasNext) {
            const nextBtn = document.createElement("button");
            nextBtn.textContent = "Sau";
            nextBtn.onclick = () => loadBlogs(pageIndex + 1);
            container.appendChild(nextBtn);
        }
    }
};
