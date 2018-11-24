using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using PersonalPlace.Domain.Base;
using PersonalPlace.Domain.Base.Component;
using PersonalPlace.Domain.Common.Exception;
using PersonalPlace.Domain.Common.ValueObjects;

namespace PersonalPlace.Domain.Entities.Catalog
{
    [EntityParameter(Segregation.Descending, isAggregateRoot: true)]
    public class Realty : EntityWithStateRecord<RealtyStateType>
    {
        protected Realty()
        {
            InitializeInternalCollections();
        }

        public Realty(Client client,
                      Address address,
                      string description,
                      RealtyStateType realtyStateType = RealtyStateType.Created,
                      string scopeTag = null)
            : this(client, address, description, scopeTag ?? client.ScopeTag, new EntityState<RealtyStateType>(realtyStateType))
        {
        }

        public Realty(Client client,
                      Address address,
                      string description,
                      string scopeTag,
                      EntityState<RealtyStateType> state = null)
            : base(state ?? new EntityState<RealtyStateType>(RealtyStateType.Created), scopeTag)
        {
            Client = client;
            Address = address;
            Description = description;
            PostDateTime = DateTimeOffset.Now;
        }

        public virtual Client Client { get; protected set; }

        public virtual Address Address { get; protected set; }

        public virtual string AddressDetail { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Furnished { get; set; }

        public virtual bool DisabilityAccess { get; set; }

        public virtual int? TotalRooms { get; set; }

        public virtual int? TotalSuites { get; set; }

        public virtual int? Age { get; set; }

        public virtual decimal? RentValue { get; set; }

        public virtual decimal? SaleValue { get; set; }

        public virtual DateTimeOffset PostDateTime { get; protected set; }

        private ISet<Floorplan> _floorplans;
        public virtual IEnumerable<Floorplan> Floorplans => _floorplans;

        private ISet<Amenity> _amenities;
        public virtual IEnumerable<Amenity> Amenities => _amenities;


        private ISet<Comment> _comments;
        public virtual IEnumerable<Comment> Comments => _comments;

        private ISet<RealtyImage> _images;
        public virtual IEnumerable<RealtyImage> Images => _images;

        public virtual void AddFloorplan(Floorplan floorplan)
        {
            if (floorplan.Realty.Id != Id)
                throw new DomainEntityException("Attempt to add a floorplan that does not belong to the realty");

            if (_floorplans.Contains(floorplan))
                return;

            _floorplans.Add(floorplan);
        }

        public virtual void AddImage(RealtyImage realtyImage)
        {
            if (realtyImage == null)
                throw new DomainEntityException("Attempt to add a image to the realty");

            if (!(Uri.TryCreate(realtyImage.Url, UriKind.Absolute, out var uriResult) && uriResult.Scheme == Uri.UriSchemeHttp))
                throw new DomainEntityException("Invalid realty image url");

            if (_images.All(x => x.Url != realtyImage.Url))
                _images.Add(realtyImage);
        }

        public virtual void AddAmenity(Amenity amenity)
        {
            if (amenity.Realty.Id != Id)
                throw new DomainEntityException("Attempt to add a amenity that does not belong to the realty");

            if (_amenities.Contains(amenity))
                return;

            _amenities.Add(amenity);
        }

        public virtual void RemoveAmenity(Amenity amenity)
        {
            if (!_amenities.Contains(amenity))
                return;

            _amenities.Remove(amenity);
        }
        public virtual void AddComment(Comment comment)
        {
            if (comment.Realty.Id != Id)
                throw new DomainEntityException("Attempt to add a comment that does not belong to the realty");

            if (_comments.Contains(comment))
                return;

            _comments.Add(comment);
        }

        public virtual void RemoveComment(Comment comment)
        {
            if (!_comments.Contains(comment))
                return;

            _comments.Remove(comment);
        }

        public virtual void RemoveImage(RealtyImage realtyImage)
        {
            if (realtyImage == null || _images.All(x => x.Url != realtyImage.Url))
                return;

            if (!(Uri.TryCreate(realtyImage.Url, UriKind.Absolute, out var uriResult) && uriResult.Scheme == Uri.UriSchemeHttp))
                throw new DomainEntityException("Invalid realty image url");

            if (_images.All(x => x.Url != realtyImage.Url))
                _images.Add(realtyImage);
        }

        public virtual void RemoveFloorplan(Floorplan floorplan)
        {
            if (!_floorplans.Contains(floorplan))
                return;

            _floorplans.Remove(floorplan);
        }

        protected override void InitializeInternalCollections()
        {
            base.InitializeInternalCollections();
            _floorplans = new HashSet<Floorplan>();
            _amenities = new HashSet<Amenity>();
            _comments = new HashSet<Comment>();
            _images = new HashSet<RealtyImage>();
        }
    }
}