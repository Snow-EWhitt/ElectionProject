using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using shared;

namespace election.Data
{
    public partial class InstantRunoffContext : DbContext
    {
        public InstantRunoffContext()
        {
        }

        public InstantRunoffContext(DbContextOptions<InstantRunoffContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ballot> Ballots { get; set; } = null!;
        public virtual DbSet<BallotPref> BallotPrefs { get; set; } = null!;
        public virtual DbSet<Candidate> Candidates { get; set; } = null!;
        public virtual DbSet<CandidateOie> CandidateOies { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<County> Counties { get; set; } = null!;
        public virtual DbSet<Election> Elections { get; set; } = null!;
        public virtual DbSet<Office> Offices { get; set; } = null!;
        public virtual DbSet<OfficeInElection> OfficeInElections { get; set; } = null!;
        public virtual DbSet<Party> Parties { get; set; } = null!;
        public virtual DbSet<State> States { get; set; } = null!;
        public virtual DbSet<Tabulationresult> Tabulationresults { get; set; } = null!;
        public virtual DbSet<Veffectiveballot> Veffectiveballots { get; set; } = null!;
        public virtual DbSet<Vresultspivot> Vresultspivots { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ballot>(entity =>
            {
                entity.ToTable("ballot", "iro");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('ballot_id_seq'::regclass)");

                entity.Property(e => e.CastTimestamp)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("cast_timestamp");

                entity.Property(e => e.Precinctinfo).HasColumnName("precinctinfo");

                entity.Property(e => e.VoterId).HasColumnName("voter_id");
            });

            modelBuilder.Entity<BallotPref>(entity =>
            {
                entity.ToTable("ballot_pref", "iro");

                entity.HasIndex(e => new { e.BallotId, e.CandidateOieId }, "ballot_candidate_oie_uix")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('ballot_pref_id_seq'::regclass)");

                entity.Property(e => e.BallotId).HasColumnName("ballot_id");

                entity.Property(e => e.CandidateOieId).HasColumnName("candidate_oie_id");

                entity.Property(e => e.PreferenceNum).HasColumnName("preference_num");

                entity.HasOne(d => d.Ballot)
                    .WithMany(p => p.BallotPrefs)
                    .HasForeignKey(d => d.BallotId)
                    .HasConstraintName("ballot_pref_ballot_fk");

                entity.HasOne(d => d.CandidateOie)
                    .WithMany(p => p.BallotPrefs)
                    .HasForeignKey(d => d.CandidateOieId)
                    .HasConstraintName("ballot_pref_candidate_oie_fk");
            });

            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.ToTable("candidate", "iro");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('candidate_id_seq'::regclass)");

                entity.Property(e => e.CandidateEmail)
                    .HasMaxLength(200)
                    .HasColumnName("candidate_email");

                entity.Property(e => e.CandidateName)
                    .HasMaxLength(200)
                    .HasColumnName("candidate_name");

                entity.Property(e => e.CandidatePhone)
                    .HasMaxLength(80)
                    .HasColumnName("candidate_phone");

                entity.Property(e => e.CandidatePhoto).HasColumnName("candidate_photo");
            });

            modelBuilder.Entity<CandidateOie>(entity =>
            {
                entity.ToTable("candidate_oie", "iro");

                entity.HasIndex(e => new { e.OieId, e.CandidateId }, "candidate_oie_nodup_uix")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('candidate_oie_id_seq'::regclass)");

                entity.Property(e => e.CandidateId).HasColumnName("candidate_id");

                entity.Property(e => e.CandidateType)
                    .HasMaxLength(20)
                    .HasColumnName("candidate_type")
                    .HasDefaultValueSql("'printed'::character varying");

                entity.Property(e => e.EliminatedTf).HasColumnName("eliminated_tf");

                entity.Property(e => e.FiledDate).HasColumnName("filed_date");

                entity.Property(e => e.OieId).HasColumnName("oie_id");

                entity.Property(e => e.PartyId).HasColumnName("party_id");

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.CandidateOies)
                    .HasForeignKey(d => d.CandidateId)
                    .HasConstraintName("candidate_oie_candidate_fk");

                entity.HasOne(d => d.Oie)
                    .WithMany(p => p.CandidateOies)
                    .HasForeignKey(d => d.OieId)
                    .HasConstraintName("candidate_oie_office_in_election_fk");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.CandidateOies)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("candidate_oie_party_fk");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city", "iro");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('city_id_seq'::regclass)");

                entity.Property(e => e.CityDescription)
                    .HasMaxLength(80)
                    .HasColumnName("city_description");

                entity.Property(e => e.CityName)
                    .HasMaxLength(80)
                    .HasColumnName("city_name");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(200)
                    .HasColumnName("contact_email");

                entity.Property(e => e.ContactName)
                    .HasMaxLength(80)
                    .HasColumnName("contact_name");

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(80)
                    .HasColumnName("contact_phone");

                entity.Property(e => e.ContactTitle)
                    .HasMaxLength(80)
                    .HasColumnName("contact_title");
            });

            modelBuilder.Entity<County>(entity =>
            {
                entity.ToTable("county", "iro");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('county_id_seq'::regclass)");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(200)
                    .HasColumnName("contact_email");

                entity.Property(e => e.ContactName)
                    .HasMaxLength(80)
                    .HasColumnName("contact_name");

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(80)
                    .HasColumnName("contact_phone");

                entity.Property(e => e.ContactTitle)
                    .HasMaxLength(80)
                    .HasColumnName("contact_title");

                entity.Property(e => e.CountyDescription)
                    .HasMaxLength(80)
                    .HasColumnName("county_description");

                entity.Property(e => e.CountyName)
                    .HasMaxLength(80)
                    .HasColumnName("county_name");
            });

            modelBuilder.Entity<Election>(entity =>
            {
                entity.ToTable("election", "iro");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('election_id_seq'::regclass)");

                entity.Property(e => e.Ballotingclosed).HasColumnName("ballotingclosed");

                entity.Property(e => e.Earlyvotingbegin).HasColumnName("earlyvotingbegin");

                entity.Property(e => e.Earlyvotingend).HasColumnName("earlyvotingend");

                entity.Property(e => e.PollDate).HasColumnName("poll_date");
            });

            modelBuilder.Entity<Office>(entity =>
            {
                entity.ToTable("office", "iro");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('office_id_seq'::regclass)");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CountyId).HasColumnName("county_id");

                entity.Property(e => e.ElectionId).HasColumnName("election_id");

                entity.Property(e => e.OfficeName)
                    .HasMaxLength(80)
                    .HasColumnName("office_name");

                entity.Property(e => e.PositionsNum).HasColumnName("positions_num");

                entity.Property(e => e.StateId).HasColumnName("state_id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Offices)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("office_city_fk");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.Offices)
                    .HasForeignKey(d => d.CountyId)
                    .HasConstraintName("office_county_fk");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Offices)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("office_state_fk");
            });

            modelBuilder.Entity<OfficeInElection>(entity =>
            {
                entity.ToTable("office_in_election", "iro");

                entity.HasIndex(e => new { e.ElectionId, e.OfficeId }, "office_in_election_nodup_uix")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('office_in_election_id_seq'::regclass)");

                entity.Property(e => e.ElectionId).HasColumnName("election_id");

                entity.Property(e => e.OfficeId).HasColumnName("office_id");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasDefaultValueSql("1");

                entity.HasOne(d => d.Election)
                    .WithMany(p => p.OfficeInElections)
                    .HasForeignKey(d => d.ElectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("office_election_election_fk");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.OfficeInElections)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("office_election_office_fk");
            });

            modelBuilder.Entity<Party>(entity =>
            {
                entity.ToTable("party", "iro");

                entity.HasIndex(e => e.PartyBallotLabel, "party_label_uix")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('party_id_seq'::regclass)");

                entity.Property(e => e.PartyBallotLabel)
                    .HasMaxLength(20)
                    .HasColumnName("party_ballot_label");

                entity.Property(e => e.PartyName)
                    .HasMaxLength(80)
                    .HasColumnName("party_name");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("state", "iro");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('state_id_seq'::regclass)");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(200)
                    .HasColumnName("contact_email");

                entity.Property(e => e.ContactName)
                    .HasMaxLength(80)
                    .HasColumnName("contact_name");

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(80)
                    .HasColumnName("contact_phone");

                entity.Property(e => e.ContactTitle)
                    .HasMaxLength(80)
                    .HasColumnName("contact_title");

                entity.Property(e => e.StateDescription)
                    .HasMaxLength(80)
                    .HasColumnName("state_description");

                entity.Property(e => e.StateName)
                    .HasMaxLength(80)
                    .HasColumnName("state_name");
            });

            modelBuilder.Entity<Tabulationresult>(entity =>
            {
                entity.ToTable("tabulationresult", "iro");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('tabulationresult_id_seq'::regclass)");

                entity.Property(e => e.CandidateOieId).HasColumnName("candidate_oie_id");

                entity.Property(e => e.Pctvotesforoffice)
                    .HasPrecision(8, 5)
                    .HasColumnName("pctvotesforoffice");

                entity.Property(e => e.Votesreceived).HasColumnName("votesreceived");

                entity.Property(e => e.VotingRound)
                    .HasMaxLength(20)
                    .HasColumnName("voting_round");

                entity.HasOne(d => d.CandidateOie)
                    .WithMany(p => p.Tabulationresults)
                    .HasForeignKey(d => d.CandidateOieId)
                    .HasConstraintName("tabulation_candidateoffice_fk");
            });

            modelBuilder.Entity<Veffectiveballot>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("veffectiveballot", "iro");

                entity.Property(e => e.CandidateId).HasColumnName("candidate_id");

                entity.Property(e => e.CoieId).HasColumnName("coie_id");

                entity.Property(e => e.ElectionId).HasColumnName("election_id");

                entity.Property(e => e.Numvotes).HasColumnName("numvotes");

                entity.Property(e => e.OfficeTotalVotes).HasColumnName("office_total_votes");

                entity.Property(e => e.OieId).HasColumnName("oie_id");

                entity.Property(e => e.Pcteffectiveballots).HasColumnName("pcteffectiveballots");
            });

            modelBuilder.Entity<Vresultspivot>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vresultspivot", "iro");

                entity.Property(e => e.CandidateId).HasColumnName("candidate_id");

                entity.Property(e => e.CandidateName)
                    .HasMaxLength(200)
                    .HasColumnName("candidate_name");

                entity.Property(e => e.ElectionId).HasColumnName("election_id");

                entity.Property(e => e.Initialroundpct).HasColumnName("initialroundpct");

                entity.Property(e => e.Initialroundvotes).HasColumnName("initialroundvotes");

                entity.Property(e => e.OfficeId).HasColumnName("office_id");

                entity.Property(e => e.OfficeName)
                    .HasMaxLength(80)
                    .HasColumnName("office_name");

                entity.Property(e => e.Round1pct).HasColumnName("round1pct");

                entity.Property(e => e.Round1votes).HasColumnName("round1votes");

                entity.Property(e => e.Round2pct).HasColumnName("round2pct");

                entity.Property(e => e.Round2votes).HasColumnName("round2votes");

                entity.Property(e => e.Round3pct).HasColumnName("round3pct");

                entity.Property(e => e.Round3votes).HasColumnName("round3votes");

                entity.Property(e => e.Round4pct).HasColumnName("round4pct");

                entity.Property(e => e.Round4votes).HasColumnName("round4votes");

                entity.Property(e => e.Round5pct).HasColumnName("round5pct");

                entity.Property(e => e.Round5votes).HasColumnName("round5votes");

                entity.Property(e => e.Round6pct).HasColumnName("round6pct");

                entity.Property(e => e.Round6votes).HasColumnName("round6votes");

                entity.Property(e => e.Round7pct).HasColumnName("round7pct");

                entity.Property(e => e.Round7votes).HasColumnName("round7votes");

                entity.Property(e => e.Round8pct).HasColumnName("round8pct");

                entity.Property(e => e.Round8votes).HasColumnName("round8votes");

                entity.Property(e => e.Round9pct).HasColumnName("round9pct");

                entity.Property(e => e.Round9votes).HasColumnName("round9votes");
            });

            modelBuilder.HasSequence("voter_id_seq", "iro").StartsAt(111);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
