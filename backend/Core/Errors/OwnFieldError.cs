using System;

namespace backend.Core.Errors
{
    public class OwnFieldError
    {
        public string Field { get; set; }
        public string Message { get; set; }

        // Constructor with parameters
        public OwnFieldError(string field, string message)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        // Builder pattern (optional, for fluent creation of the object)
        public class Builder
        {
            private string _field = "";
            private string _message = "";

            public Builder SetField(string field)
            {
                _field = field;
                return this;
            }

            public Builder SetMessage(string message)
            {
                _message = message;
                return this;
            }

            public OwnFieldError Build()
            {
                return new OwnFieldError(_field, _message);
            }
        }
    }
}
