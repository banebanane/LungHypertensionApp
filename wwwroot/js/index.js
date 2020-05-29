const tabs = document.querySelectorAll('[data-tab-target]');
const tabContents = document.querySelectorAll('[data-tab-content]')

tabs.forEach(tab => {
    tab.addEventListener('click', () => {
        const target = document.querySelector(tab.dataset.tabTarget)
        tabContents.forEach(tabContent => {
            tabContent.classList.remove('active')
        })
        tabs.forEach(tab => {
            tab.classList.remove('active')
        })
        tab.classList.add('active')
        target.classList.add('active')
    })
})

$("#t01 tr").click(function () {
    $(this).addClass("selected").siblings().removeClass("selected");
})

$("#t01 tr").keydown(function (e) {
    e = e || window.event;
    if (e.keyCode == '38') {
        // up arrow       
        if ($(this).prev != null) {
            $(this).removeClass("selected");
            $(this).prev().addClass("selected");
            $(this).prev().focus();
            if ($(this).is(":focus")) {
                $(this).addClass("selected");
            }
        }
        
        // up arrow      
        //var nextrow = tableControlls.previousElementSibling;
        //if (nextrow != null) {
        //    tableControlls.className = "";
        //    nextrow = "selected";
        // }
    } else if (e.keyCode == '40') {
        // down arrow
        if ($(this).next != null) {
            $(this).removeClass("selected");
            $(this).next().addClass("selected");
            $(this).next().focus();
            if ($(this).is(":focus")) {
                $(this).addClass("selected");
            }
        }
    }
})


//var getInstitute = $("#btnGetInstitution");
////var formInstitute = document.getElementById("searchLabel");
//var updateInstitute = document.getElementById("updateInstitution");
//var deleteInstitute = document.getElementById("deleteInstitution");

////formInstitute.hidden = true;

//getInstitute.on("click", function () {
//    updateInstitute.hidden = true;
//    deleteInstitute.hidden = true;
//});

//var theForm = $("#inputForm"); //document.getElementById("inputForm")

////theForm.hidden = true;

//var buyDugme = $("#buyDugme");

//buyDugme.on("click", function () {
//    console.log("Dugme je kliknuto");
//});

// var productInfo = $(".product-props li");
//productInfo.on("click", function () {
//    console.log("You clicked on" + $(this).text())
//});