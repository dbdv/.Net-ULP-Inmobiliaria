const base_url = "https://localhost:7164/";

function logout() {
  sessionStorage.clear();
  fetch(base_url + "logout", {
    method: "POST",
    headers: {
      "content-type": "application/json",
    },
  })
    .then((res) => {
      if (res.status !== 200) {
        console.log("error de logout");
        // document.querySelector(".login-button").classList.add("login-error");
      }
      if (res.redirected) window.location.href = res.url;
    })
    .catch((error) => {
      console.error(error);
    });
}

function closeTempMsg() {
  document.querySelector(".tempMsg").classList.add("hidden");
}
