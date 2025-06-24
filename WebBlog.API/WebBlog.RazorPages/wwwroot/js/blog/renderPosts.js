import { renderSinglePost } from "./renderSinglePost.js";
import { setupPostActions } from "./actionPost.js";

export function renderPosts(posts) {
    const container = document.getElementById("post-container");
    posts.forEach(post => {
        const postBox = renderSinglePost(post);
        container.appendChild(postBox);
    });

    setupPostActions(container);
}
