using AutoMapper;
using System.Linq;
using cars.Controllers.Resources;
using cars.Core.Models;
using System.Collections.Generic;

namespace cars.Mapping
{
    // CONTAINS COMMENTS THAT NEEDS TO DELETED FOR DEPLOYMENT
    public class MappingProfile : Profile
    {
        // contructor
        public MappingProfile()
        {
            // create maps with different types
            // Mapping the generic QueryResult class
            CreateMap(typeof(QueryResult<>), typeof(QueryResultResource<>));
            // Photo Mapping
            CreateMap<Photo, PhotoResource>();
            // Mapping Make to MakeResource. When we call this AutoMapper scans the properties of this two types
            // and if the property names match, they can be automatically mapped by automapper. If they don't then
            // they will need to supply additional configurations
            CreateMap<Make, MakeResource>();
            CreateMap<Make, KeyValuePairResource>();
            // the make class has a collection of models. A map between Model and ModelResource
            // Note this maps are unidirectional. I can only map, Make to MakeResorces but not the other way around
            // for that I need to create a seperate map
            CreateMap<Model, KeyValuePairResource>();
            CreateMap<Feature, KeyValuePairResource>();
            CreateMap<Car, SaveCarResource>()
                .ForMember(cr => cr.Contact, opt => opt.MapFrom(c => new ContactResource { Name = c.ContactName, Email = c.ContactEmail, Phone = c.ContactPhone } )) //map contact details
                .ForMember(cr => cr.Features, opt => opt.MapFrom(c => c.Features.Select(cf => cf.FeatureId)));
            // Mapping between the Car and CarResource
            CreateMap<Car, CarResource>()
                .ForMember(cr => cr.Make, opt => opt.MapFrom(c => c.Model.Make))
                .ForMember(cr => cr.Contact, opt => opt.MapFrom(c => new ContactResource { Name = c.ContactName, Email = c.ContactEmail, Phone = c.ContactPhone } ))
                .ForMember(cr => cr.Features, opt => opt.MapFrom(c => c.Features.Select(cf => new KeyValuePairResource { Id = cf.Feature.Id, Name = cf.Feature.Name } )));
            
            // API Resource to Domain
            CreateMap<CarQueryResource, CarQuery>();
            CreateMap<SaveCarResource, Car>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.ContactName, opt => opt.MapFrom(cr => cr.Contact.Name))
                .ForMember(c => c.ContactEmail, opt => opt.MapFrom(cr => cr.Contact.Email))
                .ForMember(c => c.ContactPhone, opt => opt.MapFrom(cr => cr.Contact.Phone))
                .ForMember(c => c.Features, opt => opt.Ignore())
                .AfterMap((cr, c) => {
                    // Remove unselected features
                    // get the removedFeatures
                    var removedFeatures = c.Features.Where(f => !cr.Features.Contains(f.FeatureId)).ToList();
                    // iterate over removed features
                    foreach (var f in removedFeatures)
                        // remove this feature from our Car Object
                        c.Features.Remove(f);

                    // Add new features
                    // get the addedFeatures
                    var addedFeatures = cr.Features.Where(id => !c.Features.Any(f => f.FeatureId == id)).Select(id => new CarFeature { FeatureId = id }).ToList();
                    foreach (var f in addedFeatures)
                        // add to our features collection
                        c.Features.Add(f);

                });
        }
    }
}