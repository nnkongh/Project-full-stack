import { formatDate } from "../utils/formDate.js";

// This function renders a single blog post with its details and comment section
export function renderSinglePost(post) {
    const postBox = document.createElement("div");
    const hasImage = !!post.image;
    postBox.className = "post-box blog-container " + (hasImage ? "with-image" : "without-image");
    const createdAt = formatDate(post.createdAt);
    const imageHtml = post.image
        ? `<img src="${post.image.replace("http://", "https://")}" class="post-image clickable-image" data-id="${post.id}" style="cursor:pointer; width:100%; max-height:500px; object-fit:contain;" />`
        : "";

    postBox.innerHTML = `
            <div class="header-title">
                <h3 class="post-title">${post.title}</h3>
                <div class="action-button">
                    <button class="circle-button edit-btn" data-id=${post.id}>
                        <i class="bi bi-pencil-square" data-id="${post.id}"></i> 
                    </button>
                    <button class="circle-button delete-btn" data-id="${post.id}">
                        <i class="bi bi-trash"></i>
                    </button>
                    <button class="circle-button view-btn" data-id="${post.id}">
                        <i class="bi bi-eye"></i>
                    </button>
                </div>
            </div>
             <div class="my-content">
                    <p class="time">${createdAt}</p>
                    <p>${post.content}</p>
                 ${imageHtml}
            </div>           
          
    `;
    return postBox;

}
