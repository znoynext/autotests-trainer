document.addEventListener('DOMContentLoaded', () => {
    const forms = Array.from(document.querySelectorAll('form'));

    forms.forEach((form) => {
        const requiredFields = Array.from(
            form.querySelectorAll('input[data-val-required], textarea[data-val-required], select[data-val-required]')
        ).filter((field) => field.type !== 'hidden');

        if (requiredFields.length === 0) {
            return;
        }

        const submitButtons = Array.from(form.querySelectorAll('button[type="submit"], input[type="submit"]'));
        if (submitButtons.length === 0) {
            return;
        }

        const updateSubmitState = () => {
            const hasEmptyRequiredField = requiredFields.some((field) => field.value.trim().length === 0);
            submitButtons.forEach((button) => {
                button.disabled = hasEmptyRequiredField;
            });
        };

        requiredFields.forEach((field) => {
            field.addEventListener('input', updateSubmitState);
            field.addEventListener('change', updateSubmitState);
        });

        updateSubmitState();
    });
});
