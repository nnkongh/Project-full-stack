import { formatDate } from "../utils/formDate.js";
import { get } from "../utils/httpClient.js";
import { setupCommentForm } from "../comment/cmtCreate.js";
import { loadComments } from "../comment/cmtGetAll.js"; 
document.addEventListener("DOMContentLoaded", async function () {
    const container = document.getElementById("blog-detail");
    const urlParams = new URLSearchParams(window.location.search); // url params dùng để lấy tham số từ URL 
    const id = urlParams.get("id"); // lấy giá trị của tham số id từ URL"

    //Kiểm tra xem id có tồn tại không
    if (!id) {
        alert("Không tìm thấy id bài viết");
        window.location.href = "/Blog/Index"; s
        return;
    }
    try {
        // Lấy thông tin từ responseq
        const response = await get(`https://localhost:7141/api/blog/${id}`);
        if (!response.ok) {
            throw new Error("Không tìm thấy bài viết");
        }
        // chuyển đổi thành kiểu json
        const blog = await response.json();

        const createdAt = formatDate(blog.createdAt);
        const imageHtl = blog.image
            ? `<img src=${blog.image.replace("http://", "https://")} alt="Ảnh bài viết" id="imagePreview" class="post-image"; style="width:100%; max-height:500px; object-fit:contain;" />` : "";
        container.innerHTML = `
            <div class="page-wrapper">
                <div class="detail-wrapper">
                    <div class="detail-container">
                        <div class="blog-content">
                            <div class="header-title>
                                <p class="detail-title">${blog.title}<p>
                                <p class="time">${createdAt}</p>
                                <textarea name="Content" disabled>${blog.content} </textarea>
                            </div>

                        <div>
                          ${imageHtl}
                          <textarea name="Tags" disabled>${blog.tags}</textarea>
                        </div>

                        <div class="return-btn">
                          <button type="button" class="btn-cancel" onclick="window.location.href='/Blog/Index'">Quay về</button>
                          <button type="button" class="btn-cancel" id="comment-toggle-btn">Comment</button>
                       </div>

                   </div>

                   <div class="comment-section" style="display:none; margin-top: 20px">
                        <h3>Bình luận</h3>
                        <div id=comment-list></div>
                        <textarea id="comment-input" rows="3" placeholder="Nhập bình luận của bạn" style="width:100%; margin-top:10px"></textarea>
                        <button id="submit-comment" class="btn-submit">Gửi bình luận</button>
                   </div>

                </div>
                </div>
            </div>
            `
        document.getElementById("comment-toggle-btn").addEventListener("click", function () {
            const section = document.querySelector(".comment-section");
            section.style.display = section.style.display === "none" ? "block" : "none";

            if (section.style.display === "block") {
                loadComments(blogId); // Tải bình luận khi mở phần bình luận
            }
        })
        setupCommentForm(blog.id);
    } catch (error) {
        container.innerHTML = `<p class="text-danger">Lỗi: ${error.message}</p>`;
    }
});
