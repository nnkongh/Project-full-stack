import { get, postImg } from "../utils/httpClient.js"; 


// Phương thức lấy dữ liệu từ api và hiển thị thông tin người dùng
document.addEventListener("DOMContentLoaded", async () => {
    try {

        const res = await get(`https://localhost:7141/api/profile/index`);
        const data = await res.json();
        document.getElementById("username").textContent = data.userName;
        document.getElementById("email").textContent = data.email;
        document.getElementById("role").textContent = data.role;


        //Set Ảnh
        //<img id="avatar" src="" alt="Avatar"/>
        const avatarImage = document.getElementById("avatar");
        console.log("Profile picture URL:", data.profilePicture);
        if (data.profilePicture) { // Kiểm tra nếu có ảnh đại diện 
            avatarImage.src = data.profilePicture.replace("http://", "https://");
        } else {
            avatarImage.src = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"; // Default image if no profile picture is set
        }
    } catch (error) {
        console.error("Error fetching profile data:", error);
    }

})
// Phương thức xử lý sự kiện khi người dùng cập nhật ảnh đại diện
document.getElementById("createForm").addEventListener("submit", async function (e) {
    e.preventDefault();
    const fileInput = document.getElementById("image");
    const formData = new FormData();
    formData.append("profilePictureUrl", fileInput.files[0]); // Assuming the input field has name="Image")
    try {
        const res = await postImg(`https://localhost:7141/api/profile/update`, formData);
        if(!res.ok) {
            throw new Error("Failed to update profile picture");
        }
        const data = await res.json();
        alert("Cập nhật ảnh thành công!");
        document.getElementById("avatar").src = data.profilePictureUrl; // Update the image source
    } catch (error) {
        console.error("Lỗi khi cập nhật ảnh:", error);
        alert("Cập nhật ảnh thất bại!");
    }
});