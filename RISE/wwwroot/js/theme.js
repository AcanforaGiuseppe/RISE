(function () {
    const DARK_CLASS = 'dark-theme';
    const STORAGE_KEY = 'theme';
    const COOKIE_KEY = 'rise_theme';

    function getCookie(name) {
        const m = document.cookie.match(new RegExp('(?:^|; )' + name.replace(/([.$?*|{}()\[\]\\\/\+^])/g, '\\$1') + '=([^;]*)'));
        return m ? decodeURIComponent(m[1]) : null;
    }

    function setCookie(name, value, days) {
        const d = new Date();
        d.setTime(d.getTime() + (days * 24 * 60 * 60 * 1000));
        document.cookie = `${name}=${encodeURIComponent(value)}; expires=${d.toUTCString()}; path=/; SameSite=Lax`;
    }

    function getPreferredTheme() {
        const fromStorage = localStorage.getItem(STORAGE_KEY);
        if (fromStorage === 'dark' || fromStorage === 'light') return fromStorage;

        const fromCookie = getCookie(COOKIE_KEY);
        if (fromCookie === 'dark' || fromCookie === 'light') return fromCookie;

        return 'light';
    }

    function applyTheme(theme) {
        if (theme === 'dark') document.body.classList.add(DARK_CLASS);
        else document.body.classList.remove(DARK_CLASS);

        localStorage.setItem(STORAGE_KEY, theme);
        setCookie(COOKIE_KEY, theme, 365);

        const btn = document.getElementById('themeToggle');
        if (btn) {
            btn.innerHTML = theme === 'dark'
                ? '<i class="fas fa-sun"></i>'
                : '<i class="fas fa-moon"></i>';
            btn.setAttribute('aria-pressed', theme === 'dark' ? 'true' : 'false');
        }
    }

    function boot() {
        applyTheme(getPreferredTheme());

        const btn = document.getElementById('themeToggle');
        if (btn) {
            btn.addEventListener('click', function () {
                const isDark = document.body.classList.contains(DARK_CLASS);
                applyTheme(isDark ? 'light' : 'dark');
            });
        }
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', boot);
    } else {
        boot();
    }
})();
