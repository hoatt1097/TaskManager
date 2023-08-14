// JavaScript to make rows clickable
document.addEventListener("DOMContentLoaded", function () {
    const rows = document.querySelectorAll(".clickable-row");
    rows.forEach(row => {
        row.addEventListener("click", () => {
            const targetId = row.getAttribute("data-target");
            const icon = row.getAttribute("data-row");
            const target = document.querySelector(targetId);
            target.classList.toggle("d-none");
            $(icon).toggleClass('fa-plus fa-minus');
        });
    });
});

$(document).ready(function () {

    // Loại bỏ lớp 'active' khỏi tất cả các phần tử li
    

    if (window.location.href.includes('/Home/Index?userId=1')) {
        $('.menu li').removeClass('active');
        $('.menu .assignme').addClass('active');
    } else if (window.location.href.includes('/Home/Index')) {
        $('.menu li').removeClass('active');
        $('.menu .home').addClass('active');
    }

    // Bắt sự kiện right-click vào mỗi hàng và hiển thị dropdown menu
    $(".table-task tbody tr").on("contextmenu", function (event) {
        event.preventDefault(); // Ngăn chặn hiển thị context menu mặc định

        // Change background color
        // Xóa background color của các hàng khác
        $(".table-task tbody tr").removeClass("row-clicked");
        $(this).addClass("row-clicked");

        var contextMenu = $("#contextMenu");
        // Tính toán vị trí hiển thị của dropdown menu
        var menuX = event.pageX - 160;
        var menuY = event.pageY - 40; // Tăng giá trị y để hiển thị bên dưới chuột

        var menuWidth = contextMenu.width();

        if (menuX + 220 + menuWidth > window.innerWidth) {
            // Nếu dropdown menu không hiển thị đủ bên phải, thử hiển thị bên trái
            if (event.pageX - menuWidth > 0) {
                menuX = event.pageX - menuWidth - 160;
            }
        }

        // Kiểm tra nếu dropdown menu vượt quá chiều dọc của màn hình
        var menuHeight = contextMenu.height();
        if (menuY + menuHeight > window.innerHeight) {
            // Nếu dropdown menu không hiển thị đủ bên dưới, thử hiển thị bên trên
            if (event.pageY - menuHeight > 0) {
                menuY = event.pageY - menuHeight - 10;
            } else {
                // Nếu không đủ chỗ bên trên, hiển thị bên dưới
                menuY = window.innerHeight - menuHeight - 10;
            }
        }

        contextMenu.css({
            display: "block",
            left: menuX,
            top: menuY,
        });

        // Lưu ID của hàng được click phải
        clickedRowID = $(this).data("id");

        // Đóng context menu khi click chuột bất kỳ
        $(document).on("click", function () {
            contextMenu.css("display", "none");
            $(".table-task tbody tr").removeClass("row-clicked");
        });
    });


    // Edit content cell khi 
    let oldValue = '';
    let newValue = '';

    $('.editable').on('click', function (event) {
        if (event.originalEvent.detail === 1) {

        } else if (event.originalEvent.detail === 2) {
            // Xóa thuộc tính contenteditable của tất cả các ô trừ ô đang chỉnh sửa
            $('.editable').removeAttr('contenteditable');

            oldValue = $(this).text();
            $(this).attr('contenteditable', 'true').focus();
        }
    });


    $('.editable').on('blur', function () {
        // Remove all contenteditable 
        $('.editable').removeAttr('contenteditable');

        var rowId = $(this).closest('tr').data("id");
        newValue = $(this).text();
        // Lưu nội dung sau khi chỉnh sửa vào cơ sở dữ liệu (hoặc thực hiện xử lý phù hợp với yêu cầu của bạn)
        if (oldValue !== newValue) {
            console.log(rowId + ': ' + newValue);
        }

    });

});



















// Hide submenus
$('#body-row .collapse').collapse('hide');

// Collapse/Expand icon
$('#collapse-icon').addClass('fa-angle-double-left');

// Collapse click
$('[data-toggle=sidebar-colapse]').click(function () {
    SidebarCollapse();
});

function SidebarCollapse() {
    $('.menu-collapsed').toggleClass('d-none');
    $('.sidebar-submenu').toggleClass('d-none');
    $('.submenu-icon').toggleClass('d-none');
    $('#sidebar-container').toggleClass('sidebar-expanded sidebar-collapsed');

    // Treating d-flex/d-none on separators with title
    var SeparatorTitle = $('.sidebar-separator-title');
    if (SeparatorTitle.hasClass('d-flex')) {
        SeparatorTitle.removeClass('d-flex');
    } else {
        SeparatorTitle.addClass('d-flex');
    }

    // Collapse/Expand icon
    $('#collapse-icon').toggleClass('fa-angle-double-left fa-angle-double-right');
}

// ---------------------------------
// ---------------------------------
// ---------------------------------
// Code right menu: Task detail
// ---------------------------------
// ---------------------------------


$(document).on("keydown", function (e) {
    if (e.key === "Escape") {
        const navbarContainer = - $(".navbar-container").width();
        $(".navbar-container").css("right", navbarContainer);

        $('#project-section').addClass('d-none');
        $('#filter-section').addClass('d-none');
    }
});

$(".resizer").on("mousedown", function (e) {
    const navbarContainer = $(".navbar-container");
    const startX = e.clientX;
    const startWidth = navbarContainer.width();

    $(document).on("mousemove", function (e) {
        const diffX = e.clientX - startX;
        const newWidth = startWidth - diffX;
        navbarContainer.css("width", newWidth);

        resizeNavbar(newWidth); // Thay đổi width của nội dung navbar theo width mới
    });

    $(document).on("mouseup", function () {
        $(document).off("mousemove");
    });
});

// Gọi resizeNavbar ban đầu
resizeNavbar($(".navbar-container").width());

// Gọi resizeNavbar khi màn hình thay đổi kích thước
$(window).resize(function () {
    resizeNavbar($(".navbar-container").width());
});


$("#new-ticket").click(function () {
    const navbarContainer = $(".navbar-container");
    const startWidth = - $(".navbar-container").width();
    if (navbarContainer.css("right") !== "0px") {
        navbarContainer.css("right", "0px");
    }

    $("#task-title").html("New task");
});



$(".close-btn").click(function () {
    const navbarContainer = - $(".navbar-container").width();
    $(".navbar-container").css("right", navbarContainer);
});

function resizeNavbar(newWidth) {
    $(".navbar-right").width(newWidth);

    // Thay đổi lớp cho các cột dựa vào chiều rộng của navbar
    if (newWidth >= 992) {
        $(".col-dynamic").removeClass("col-sm-6 col-12").addClass("col-md-3");
    } else if (newWidth >= 600) {
        $(".col-dynamic").removeClass("col-md-3 col-12").addClass("col-sm-6");
    } else {
        $(".col-dynamic").removeClass("col-md-3 col-sm-6").addClass("col-12");
    }
}


// Multi select
$('#assigneeDropdown').select2();

// summernote

$('#description').summernote({
    height: 200,
    toolbar: [
        ['style', ['bold', 'italic', 'underline']], // B, I, U Buttons
        ['para', ['ul', 'ol', 'paragraph']],
        ['height', ['height']]
    ]
});

// Code right menu: Task detail
// ---------------------------------
// ---------------------------------

$('.js-show-section').click(function () {
    let toggleId = $(this).data("id");
    $('#' + toggleId).toggleClass('d-none');
});





// Select box call API filter
// Lấy button "Apply Filter"
const filterButton = $('#filter');

// Lấy checkbox "Select All" cho cả Project và Assignee
const selectAllProjectCheckbox = $('#select-all-project');
const selectAllAssigneeCheckbox = $('#select-all-assignee');
const selectAllTaskstatusCheckbox = $('#select-all-taskstatus');

// Lấy tất cả các checkbox con cho cả Project và Assignee
const projectCheckboxes = $('.filter-checkbox[name="filter-project"]');
const assigneeCheckboxes = $('.filter-checkbox[name="filter-user"]');
const taskstatusCheckboxes = $('.filter-checkbox[name="filter-taskstatus"]');

// Xử lý sự kiện khi checkbox "Select All" được click
selectAllProjectCheckbox.on('change', function () {
    const isChecked = $(this).prop('checked');
    projectCheckboxes.prop('checked', isChecked);
});

selectAllAssigneeCheckbox.on('change', function () {
    const isChecked = $(this).prop('checked');
    assigneeCheckboxes.prop('checked', isChecked);
});

selectAllTaskstatusCheckbox.on('change', function () {
    const isChecked = $(this).prop('checked');
    taskstatusCheckboxes.prop('checked', isChecked);
});

// Xử lý sự kiện khi checkbox con được click
projectCheckboxes.on('change', function () {
    const allProjectChecked = projectCheckboxes.length === projectCheckboxes.filter(':checked').length;
    selectAllProjectCheckbox.prop('checked', allProjectChecked);
});

assigneeCheckboxes.on('change', function () {
    const allAssigneeChecked = assigneeCheckboxes.length === assigneeCheckboxes.filter(':checked').length;
    selectAllAssigneeCheckbox.prop('checked', allAssigneeChecked);
});

taskstatusCheckboxes.on('change', function () {
    const allTaskstatusChecked = taskstatusCheckboxes.length === taskstatusCheckboxes.filter(':checked').length;
    selectAllTaskstatusCheckbox.prop('checked', allTaskstatusChecked);
});





// Hàm hiển thị Toast 
function showToast(type, message) {
    const color = type === 'success' ? 'green' : 'red';
    Toastify({
        text: message,
        duration: 2000,
        close: true,
        gravity: "top", // Position "top", "bottom", "left", "right"
        position: "right", // Position "left", "right", "center"
        backgroundColor: color,
        stopOnFocus: true // Prevents dismissing of toast on hover
    }).showToast();
}


