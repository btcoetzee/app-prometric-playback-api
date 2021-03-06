﻿using Newtonsoft.Json;
using Prometric.Playback.Application.DTO;
using Prometric.Playback.Core.Entities;
using Prometric.Playback.Infrastructure.Redis.Documents;

namespace Prometric.Playback.Infrastructure.Redis
{
    public static class Extensions
    {
        public static string AsJson(this RecordingDocument doc)
           => JsonConvert.SerializeObject(doc);

        public static RecordingDocument AsDocument(this Recording entity)
            => new RecordingDocument
            {
                Id = entity.Id,
                TrackName = entity.TrackName
            };

        public static RecordingDocument AsRecordingDocument(this string json)
            => JsonConvert.DeserializeObject<RecordingDocument>(json);

        public static Recording AsEntity(this RecordingDocument doc)
            => new Recording(doc.Id, doc.TrackName);

        public static RecordingDto AsDto(this Recording entity)
            => new RecordingDto()
            {
                RecordingId = entity.Id,
                TrackName = entity.TrackName
            };
        public static string AsJson(this CompositionDocument doc)
     => JsonConvert.SerializeObject(doc);

        public static CompositionDocument AsDocument(this Composition entity)
            => new CompositionDocument
            {
                Id = entity.Id,
                CompositionName = entity.CompositionName
            };

        public static CompositionDocument AsCompositionDocument(this string json)
            => JsonConvert.DeserializeObject<CompositionDocument>(json);

        public static Composition AsEntity(this CompositionDocument doc)
            => new Composition(doc.Id, doc.CompositionName);

        public static CompositionDto AsDto(this Composition entity)
            => new CompositionDto()
            {
                CompositionId = entity.Id,
                CompositionName = entity.CompositionName
            };
    }
}
