function wrapFormInFetch(formId){
	document.getElementById(formId).addEventListener('submit', async function (event) {
		event.preventDefault(); // Prevent traditional form submission

		const form = event.target;
		const formData = new FormData(form);

		try {
			const response = await fetch(form.action, {
				method: form.method,
				body: formData,
				headers: {
					'MY-MVC-SECURITY-CRUTCH-HEADER': 'CRUTCH'
				}
			});

			if (response.ok) {
				// Get the response text (assuming HTML content is returned)
				const result = await response.text();

				// Replace the entire document content
				document.open();               // Clear the current document
				document.write(result);        // Write new content
				document.close();              // Finish the document

				// Change the URL without reloading the page
				history.pushState(null, '', form.action);
			} else {
				const result = await response.text();
				// Handle error by showing in resultMessage div
				document.getElementById('resultMessage').innerHTML = 'Error: ' + result;
			}
		} catch (error) {
			document.getElementById('resultMessage').innerHTML = 'Error: ' + error.message;
		}
	});
}
