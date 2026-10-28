// Kvarter - small progressive enhancements. Keep this file tiny (Lighthouse!).
// (the 'js' class on <html> is set inline in _Layout.cshtml to avoid a menu flash)

(function () {
    var toggle = document.querySelector('.nav-toggle');
    var nav = document.getElementById('site-nav');
    if (!toggle || !nav) return;

    toggle.addEventListener('click', function () {
        var open = toggle.getAttribute('aria-expanded') === 'true';
        toggle.setAttribute('aria-expanded', String(!open));
        nav.classList.toggle('is-open', !open);
    });

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && nav.classList.contains('is-open')) {
            toggle.setAttribute('aria-expanded', 'false');
            nav.classList.remove('is-open');
            toggle.focus();
        }
    });
})();
