using Dometrain.Monolith.Api.Enrollments;
using MassTransit;

namespace Dometrain.Monolith.Api.Orders;

public record OrderPlaced(Guid OrderId, Guid StudentId, IEnumerable<Guid> CourseIds);

public class OrderPlacedConsumer : IConsumer<OrderPlaced>
{
    private readonly IEnrollmentService _enrollmentService;

    public OrderPlacedConsumer(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    public async Task Consume(ConsumeContext<OrderPlaced> context)
    {
        var enrollments = await _enrollmentService.GetStudentEnrollmentsAsync(context.Message.StudentId);
        
        foreach (var courseId in context.Message.CourseIds.Where(x => !enrollments!.CourseIds.Contains(x)))
        {
            await _enrollmentService.EnrollToCourseAsync(context.Message.StudentId, courseId);
        }
    }
}
