using EmployeeManagementSystemApp.Week6.Dtos;
using FluentValidation;

namespace EmployeeManagementSystemApp.Week6.Validators
{
    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator()
        {
            RuleFor(request => request.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(1, 50);

            RuleFor(request => request.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(1, 50);

            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.")
                .MaximumLength(100);

            RuleFor(request => request.Salary)
                .InclusiveBetween(0.01m, 9999999m)
                .WithMessage("Salary must be between 0.01 and 9,999,999.");

            RuleFor(request => request.DepartmentId)
                .GreaterThan(0).WithMessage("Department ID must be greater than zero.");

            RuleForEach(request => request.Skills).SetValidator(new CreateSkillRequestValidator());
        }
    }

    public class CreateSkillRequestValidator : AbstractValidator<CreateSkillRequest>
    {
        public CreateSkillRequestValidator()
        {
            RuleFor(skill => skill.Name)
                .NotEmpty().WithMessage("Skill name is required.")
                .Length(1, 50);

            RuleFor(skill => skill.Years)
                .InclusiveBetween(0, 40)
                .WithMessage("Skill years must be between 0 and 40.");
        }
    }

    public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
    {
        public UpdateEmployeeRequestValidator()
        {
            RuleFor(request => request.FirstName).NotEmpty().Length(1, 50);
            RuleFor(request => request.LastName).NotEmpty().Length(1, 50);
            RuleFor(request => request.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(request => request.Salary).InclusiveBetween(0.01m, 9999999m);
            RuleFor(request => request.DepartmentId).GreaterThan(0);
        }
    }

    public class PatchEmployeeRequestValidator : AbstractValidator<PatchEmployeeRequest>
    {
        public PatchEmployeeRequestValidator()
        {
            RuleFor(request => request)
                .Must(request =>
                    request.FirstName is not null ||
                    request.LastName is not null ||
                    request.Email is not null ||
                    request.Salary is not null ||
                    request.DepartmentId is not null)
                .WithMessage("At least one field is required for PATCH.");

            RuleFor(request => request.FirstName).Length(1, 50).When(request => request.FirstName is not null);
            RuleFor(request => request.LastName).Length(1, 50).When(request => request.LastName is not null);
            RuleFor(request => request.Email).EmailAddress().MaximumLength(100).When(request => request.Email is not null);
            RuleFor(request => request.Salary).InclusiveBetween(0.01m, 9999999m).When(request => request.Salary.HasValue);
            RuleFor(request => request.DepartmentId).GreaterThan(0).When(request => request.DepartmentId.HasValue);
        }
    }

    public class GiveRaiseRequestValidator : AbstractValidator<GiveRaiseRequest>
    {
        public GiveRaiseRequestValidator()
        {
            RuleFor(request => request.Percent)
                .InclusiveBetween(0.01m, 100m)
                .WithMessage("Raise percent must be between 0.01 and 100.");
        }
    }

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(request => request.UserName).NotEmpty().MaximumLength(50);
            RuleFor(request => request.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
        }
    }

    public class EmployeeQueryParametersValidator : AbstractValidator<EmployeeQueryParameters>
    {
        private static readonly string[] AllowedSortFields =
        {
            "employeeid", "firstname", "lastname", "email", "salary", "joiningdate", "departmentid"
        };

        public EmployeeQueryParametersValidator()
        {
            RuleFor(query => query.Page).GreaterThan(0);
            RuleFor(query => query.PageSize).InclusiveBetween(1, 50);
            RuleFor(query => query.DepartmentId).GreaterThan(0).When(query => query.DepartmentId.HasValue);
            RuleFor(query => query.MinSalary).GreaterThan(0).When(query => query.MinSalary.HasValue);
            RuleFor(query => query.MaxSalary).GreaterThan(0).When(query => query.MaxSalary.HasValue);
            RuleFor(query => query)
                .Must(query => !query.MinSalary.HasValue || !query.MaxSalary.HasValue || query.MinSalary <= query.MaxSalary)
                .WithMessage("MinSalary cannot be greater than MaxSalary.");
            RuleFor(query => query.SortBy)
                .Must(sortBy => AllowedSortFields.Contains(sortBy.Trim().ToLowerInvariant()))
                .WithMessage("SortBy must be employeeId, firstName, lastName, email, salary, joiningDate, or departmentId.");
            RuleFor(query => query.SortDir)
                .Must(dir => dir.Equals("asc", StringComparison.OrdinalIgnoreCase) || dir.Equals("desc", StringComparison.OrdinalIgnoreCase))
                .WithMessage("SortDir must be asc or desc.");
        }
    }
}
