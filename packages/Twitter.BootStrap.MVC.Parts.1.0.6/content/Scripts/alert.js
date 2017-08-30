function ShowAlert(message) {
  $(".alert > strong").html(message);
  $(".alert").attr("class", "alert alert-success").css("display", "block");
}

function ShowWarning(message) {
  $(".alert > strong").html(message);
  $(".alert").attr("class", "alert alert-block").css("display", "block");
}

function ShowError(message) {
  $(".alert > strong").html(message);
  $(".alert").attr("class", "alert alert-error").css("display", "block");
}

function ShowModal(header, message, okfunction, cancelfunction) {
  $("#_part_modal_dialog > .modal-header > h3").text(header);
  $("#_part_modal_dialog > .modal-body > p").text(message);

  $("#_part_modal_dialog_ok").click(function () { $("#_part_modal_dialog").modal("hide"); if (okfunction != null) okfunction(); });
  $("#_part_modal_dialog_cancell").click(function () { $("#_part_modal_dialog").modal("hide"); if (cancelfunction != null) cancelfunction(); });

  $("#_part_modal_dialog").modal({ keyboard: false });
}
