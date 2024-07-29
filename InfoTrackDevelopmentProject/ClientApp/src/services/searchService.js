export const search = async (keywords, url) => {
    const response = await fetch('api/search', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ keywords, url })
    });
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    return await response.json();
};