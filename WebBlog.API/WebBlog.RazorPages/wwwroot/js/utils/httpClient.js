

/*
  nơi đóng gói sẵn các hàm dùng chung để gọi API, xử lý sự kiện, v.v. nhờ đó không cần viết lại fetch()
 */
//fetch 
async function handleResponse(res) {
    if (!res.ok) {
        const errorText = await res.text();
        throw new Error(`Error: ${res.status} - ${errorText}`);
    }
    return res;
}
export async function get(url) {
    const res = await fetch(url, {
        method: "GET",
        credentials: "include",
    });
    return handleResponse(res);
}
export async function post(url, data) {
    const res = await fetch(url, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data),
    })
    return handleResponse(res);
}
export async function blogIndex(url, page, pageSize) {
    const fullUrl = `${url}?pageIndex=${page}&pageSize=${pageSize}`
    const res = await fetch(fullUrl, {
        method: "GET",
        credentials: "include",
    });
    return handleResponse(res);
}
export async function postForm(url, dataForm) {
    const res = await fetch(url, {
        method: "POST",
        credentials: "include",
        body: dataForm 

    });
    return handleResponse(res);
}

export async function remove(url) {
    const res = await fetch(url, {
        method: "DELETE",
        credentials: "include",
    });
    return res;
}
export async function put(url, data) {
    const res = await fetch(url, {
        method: "PUT",
        credentials: "include",
        body: data,
    });
    return res;
}