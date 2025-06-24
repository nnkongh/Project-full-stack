import { formatDate } from '../utils/formDate.js';
import { get } from '../utils/httpClient.js';

export async function loadComments(blogId) {
    const commentList = document.getElementById("comment-list");
    commentList.innerHTML = "";

    try {
        const res = await get(`https://localhost:7141/api/comment/Get${blogId}`);
        if (!res.ok) {
            throw new Error("Không thể tải bình luận");
        }
        const comments = await res.json();
        if (comments.length === 0) {
            commentList.innerHTML = "<p>Chưa có bình luận nào.</p>";
        }        comments.forEach(cmt => appendComment(cmt, commentList));
    }
    catch (error) {
        commentList.innerHTML = `<p class="text-danger">${err.message}</p>`;

    }

}
function appendComment(comment, container) {
    const div = document.createElement("div");
    div.innerHTML = `
    <p><strong>User ${comment.userId}:</strong> ${comment.content}</p>
        <small>${formatDate(comment.createdAt)}</small>
        <hr/>`;
    container.appendChild(div);
}