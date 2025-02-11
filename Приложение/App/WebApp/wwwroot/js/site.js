const buttons = document.querySelectorAll('.event-button')

buttons.forEach(button => {
    button.addEventListener('click', async () => {
        const text = button.getAttribute('data-ics');

        const file = await window.showSaveFilePicker({
            types: [{accept: {'text/plain':'.ics'}}]
        })
        
        const stream = await file.createWritable();
        await stream.write(text);
        await stream.close();
        alert('Файл сохранен!')
    })
})