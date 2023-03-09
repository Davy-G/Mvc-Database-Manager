const { error } = require("jquery")


function ValidateData() {
 

        var Username = document.getElementById("Username").value
        var Password = document.getElementById("password").value
        
    if (Username == "" || Password.length == "") {
        toastr.error("You Entered Empty Fields!")
        return
    }


   
    
   


}