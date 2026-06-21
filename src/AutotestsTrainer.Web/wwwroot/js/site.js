document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.querySelector('[data-login-form]');
    if (!loginForm) {
        return;
    }

    const fields = Array.from(loginForm.querySelectorAll('[data-login-field]'));
    const submitButton = loginForm.querySelector('[data-login-submit]');
    if (!submitButton || fields.length === 0) {
        return;
    }

    const updateSubmitState = () => {
        submitButton.disabled = fields.some((field) => field.value.trim().length === 0);
    };

    fields.forEach((field) => {
        field.addEventListener('input', updateSubmitState);
        field.addEventListener('change', updateSubmitState);
    });

    updateSubmitState();
});
