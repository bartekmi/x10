namespace x10.schema.validation {
    public class ValidationMandatory : Validation {
        public ValidationMandatory() {
            ErrorMessage = "Field is required";
            Function = "isPresent";
        }
    }
}