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
    var row = $(this).index();
    var idControll = document.getElementById("t01").rows[row].cells[0].innerText;
    var controllDate = document.getElementById("t01").rows[row].cells[1].innerText;
    var weekHearth = document.getElementById("t01").rows[row].cells[2].innerText;
    document.getElementById("idControll").value = idControll;
    document.getElementById("controllDate").value = controllDate;
    document.getElementById("weekHearth").value = weekHearth;
    $(this).addClass("selected").siblings().removeClass("selected");
})

$("#t02 tr").click(function () {
    var row = $(this).index();
    var patientId = document.getElementById("t02").rows[row].cells[0].innerText;
    var firstName = document.getElementById("t02").rows[row].cells[1].innerText;
    var lastName = document.getElementById("t02").rows[row].cells[2].innerText;
    document.getElementById("id").value = patientId;
    document.getElementById("firstName").value = firstName;
    document.getElementById("lastName").value = lastName;
    document.getElementById("id1").value = patientId;
    document.getElementById("firstName1").value = firstName;
    document.getElementById("lastName1").value = lastName;
    $(this).addClass("selected").siblings().removeClass("selected");
})

$("#allInstitutionsForUser").change(function () {
    var e = document.getElementById("allInstitutionsForUser");
    var selectedInstitution = e.options[e.selectedIndex].text;
    window.location.href = $'@Url.Action("ChangedModel", "Patient", new{ institutionName =  })'
    window.location.href = "/Patient/ChangedModel/institutionName=" + selectedInstitution;
})

//$("#t01 tr").keydown(function (e) {
//    e = e || window.event;
//    var row = $(this).index();
//    if (e.keyCode == '38' && row > 1) {
//        // up arrow       
//        if ($(this).prev != null) {
//            $(this).removeClass("selected");
//            $(this).prev().addClass("selected");
//            $(this).prev().focus();
//            var idControll = document.getElementById("t01").rows[row -1].cells[0].innerText;
//            var controllDate = document.getElementById("t01").rows[row -1].cells[1].innerText;
//            var weekHearth = document.getElementById("t01").rows[row-1].cells[2].innerText;
//            document.getElementById("idControll").value = idControll;
//            document.getElementById("controllDate").value = controllDate;
//            document.getElementById("weekHearth").value = weekHearth;
//            if ($(this).is(":focus")) {
//                $(this).addClass("selected");
//           }
//        }
//    } else if (e.keyCode == '40') {
//        // down arrow
//        if ($(this).next != null) {
//            $(this).removeClass("selected");
//            $(this).next().addClass("selected");
//            $(this).next().focus();
//            if ($(this).is(":focus")) {
//                $(this).addClass("selected");
//            }
//            var idControll = document.getElementById("t01").rows[row +1].cells[0].innerText;
//            var controllDate = document.getElementById("t01").rows[row +1].cells[1].innerText;
//            var weekHearth = document.getElementById("t01").rows[row +1].cells[2].innerText;
//            document.getElementById("idControll").value = idControll;
//            document.getElementById("controllDate").value = controllDate;
//            document.getElementById("weekHearth").value = weekHearth;
//        }
//    }
//})