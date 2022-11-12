//const navLinks = document.querySelectorAll('.menu-item');

//navLinks.forEach(link => {
//    link.addEventListener('click', function handleClick(event) {
//        navLinks.forEach(link => {
//            link.classList.remove("current")
//        })
//        link.classList.add("current")
//        console.log('link clicked', event);

//    });
//});
const homePage = document.getElementById("page-id-home")
const closetPage = document.getElementById("page-id-closet")
if (homePage != null) {
    document.getElementById("home-nav-link").classList.add("current")
} else if (closetPage != null) {
    document.getElementById("closet-nav-link").classList.add("current")
}
