// createCmt.js
import { post } from "../utils/httpClient.js";
import { loadComments } from "./cmtGetAll.js";
export function setupCommentForm(blogId) {
    const input = document.getElementById("comment-input");
    const submitBtn = document.getElementById("submit-comment");

    submitBtn.addEventListener("click", async function () {
        const content = input.value.trim();
        if (!content) {
            alert("Nội dung bình luận không được để trống");
            return;
        }
        try {
            const res = await post(`https://localhost:7141/api/comment/Create/${blogId} `, {
                content: content
            });
            if (!res.ok) {
                throw new Error("Không thể gửi bình luận");
            }
            input.value = ""; // Xóa nội dung input sau khi gửi
            await loadComments(blogId); // Tải lại bình luận
        } catch (error) {
            alert(error.message);
        }
    });
    
    
}
