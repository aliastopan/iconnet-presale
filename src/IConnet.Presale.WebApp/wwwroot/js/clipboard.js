window.readClipboard = async () => {
    try {
        const text = await navigator.clipboard.readText();
        console.log('Reading clipboard contents');
        return text;
    } catch (error) {
        console.error('Failed to read clipboard contents: ', error);
        return null;
    }
};