function ConfirmOnDelete(a) {
    if (confirm("Are you sure that you want to delete this role?") === true)
        return true;
    else
        return false;

 };