//overall function to hold search, background, and time functions
$(document).ready(function () {
    const apiKey = _config["apiKey:Secret"];  
    const cx = _config["cx:Secret"];
    //search function
    $("#searchBtn").click(function () {
        const query = $("#query").val();
        if (!query) return;

        $.get("https://www.googleapis.com/customsearch/v1", {
            key: apiKey,
            cx: cx,
            q: query
        }).done(function (data) {
            let results = "";
            if (data.items) {
                //display of results
                data.items.forEach(item => {
                    results += `
                        <div class="result">
                            <h3><a href="${item.link}" target="_blank">${item.title}</a></h3>
                            <p>${item.snippet}</p>
                        </div>`; 
                    });
            } 
            else {  results = "<p>No results found.</p>";}
            $("#searchResults").html(results).css("display", "block");

        }).fail(function (err) {console.error("Search failed:", err);    }); });
    //background rotation
    const backgrounds = [
        "css/pic1.jpg",
        "css/pic2.jpg",
        "css/pic3.jpg", 
        "css/pic4.jpg"
    ];
    let bgIndex = 0;

    $(".siteHeader").click(function () {
        bgIndex = (bgIndex + 1) % backgrounds.length;
        $("body").css("background-image", `url('${backgrounds[bgIndex]}')`);
    });

    //time display 
     $("#timeBtn").click(function () {
        const now = new Date();
        const hours = String(now.getHours()).padStart(2, "0");
        const minutes = String(now.getMinutes()).padStart(2, "0");
        const formatted = `${hours}:${minutes}`;

        $("#time").html(`<div class="time-dialog-content">
                        <h2>Current Time</h2>
                        <p>${formatted}</p>
                         </div>
        `).dialog({
            modal: true,
            width: 300,
            height: "auto",
            show: { effect: "fade", duration: 300 },
            hide: { effect: "fade", duration: 300 },
            buttons: {
                Close: function () {    $(this).dialog("close");}}}); });
});
